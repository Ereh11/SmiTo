using SmiTo.Application.DTOs.Auth;
using SmiTo.Application.Interfaces;
using SmiTo.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.Services;

public class UserService: IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GeneralResult<List<GetAllUsersResponse>>?> GetAllUsers(int page, int pageSize)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(page, pageSize);
        if (users == null || !users.Any())
        {
            return GeneralResult<List<GetAllUsersResponse>>.Failure(new List<string> { "No users found." }, "No users found.");
        }
        var userResponses = users.Select(user => new GetAllUsersResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        }).ToList();
        return GeneralResult<List<GetAllUsersResponse>>.SuccessResult(userResponses, "Users retrieved successfully.");
    }
}
