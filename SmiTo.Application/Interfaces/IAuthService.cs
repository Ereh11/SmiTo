using SmiTo.Application.DTOs.Auth;

namespace SmiTo.Application.Interfaces;

public interface IAuthService
{
    Task<GeneralResult> RegisterAsync(RegisterRequest request);
    Task<GeneralResult> LoginAsync(LoginRequest request);
    Task AssignRoleAsync(AssignRoleRequest request, string adminId);
}
