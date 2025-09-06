using SmiTo.Domain.Entities;

namespace SmiTo.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<bool> IsEmailExist(string email);
    Task<IEnumerable<User>?> GetAllAsync(int page, int pageSize);
}
