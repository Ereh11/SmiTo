using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmiTo.Application.DTOs.Auth;
using SmiTo.Application.Interfaces;
using SmiTo.Domain.Entities;
using SmiTo.Domain.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
                       IUnitOfWork unitOfWork,
                       UserManager<User> userManager,
                       SignInManager<User> signInManager,
                       RoleManager<IdentityRole<Guid>> roleManager,
                       IOptions<JwtSettings> jwtSettings,
                       ILogger<AuthService> logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }
    public async Task<GeneralResult> RegisterAsync(RegisterRequest request)
    {
     
        bool isEmailExist = await _userManager.FindByEmailAsync(request.Email) != null;
        if (isEmailExist) 
        {
            return GeneralResult.Failure(new List<string> { "Email is already in use" }, "Registration failed");
        }

        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return GeneralResult.Failure(errors, "User registration failed");
        }

        if (!await _roleManager.RoleExistsAsync("User"))
            await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));

        await _userManager.AddToRoleAsync(user, "User");

        var token = await GenerateJwtToken(user);
        return GeneralResult<AuthResponse>.SuccessResult(token, "User registered successfully");
    }

    public async Task<GeneralResult> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        { 
            return GeneralResult.Failure(new List<string> { "Invalid email or password" }, "Login failed");
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
        {
            return GeneralResult.Failure(new List<string> { "Invalid email or password" }, "Login failed");
        }
        var token = await GenerateJwtToken(user);
        return GeneralResult<AuthResponse>.SuccessResult(token, "Login successful");
    }
    public async Task AssignRoleAsync(AssignRoleRequest request, string adminId)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) throw new Exception("User not found");

        if (!await _roleManager.RoleExistsAsync(request.RoleName))
            throw new Exception("Role does not exist");

        if (await _userManager.IsInRoleAsync(user, request.RoleName))
            throw new Exception($"User already has role '{request.RoleName}'");

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

        _logger.LogInformation("Admin {AdminId} assigned role {Role} to user {UserId}",
            adminId, request.RoleName, user.Id);
    }
    private async Task<AuthResponse> GenerateJwtToken(User user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            signingCredentials: creds);

        return new AuthResponse(new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }
}
