using SmiTo.Application.DTOs.Auth;

namespace SmiTo.Application.Interfaces;

public interface IUserService
{
    Task<GeneralResult<List<GetAllUsersResponse>>?> GetAllUsers(int page, int pageSize);
}
