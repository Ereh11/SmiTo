using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SmiTo.Application.DTOs.Auth;
using SmiTo.Application.Interfaces;
using SmiTo.Domain.Repositories;

namespace SmiTo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        public UserController(IAuthService authService,
            IUserService userService
            )
        {
            _authService = authService;
          _userService = userService;

        }
        [HttpPost("auth/register")]
        public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> Register([FromBody] SmiTo.Application.DTOs.Auth.RegisterRequest registerRequest)
        {
            var response = await _authService.RegisterAsync(registerRequest);
            if (response.Success)
                return TypedResults.Ok(response);
            return TypedResults.BadRequest(response);
        }
        [HttpPost("auth/login")]
        public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> Login([FromBody] SmiTo.Application.DTOs.Auth.LoginRequest loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);
            if (response.Success)
                return TypedResults.Ok(response);
            return TypedResults.BadRequest(response);
        }
        [HttpGet]
        public async Task<Results<Ok<GeneralResult<List<GetAllUsersResponse>>>, NotFound>> GetAllUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetAllUsers(page, pageSize);
            if (response is not null && response.Success)
                return TypedResults.Ok(response);
            return TypedResults.NotFound();
        }
    }
}
