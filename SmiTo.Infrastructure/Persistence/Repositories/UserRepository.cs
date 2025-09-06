using Microsoft.EntityFrameworkCore;
using SmiTo.Domain.Entities;
using SmiTo.Domain.Repositories;
using SmiTo.Infrastructure.Persistence.Context;

namespace SmiTo.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SmiToDbContext _context;
    public UserRepository(SmiToDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>?> GetAllAsync(int page = 1, int pageSize = 8)
    {
        return await _context.Users
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<bool> IsEmailExist(string email)
    {
        return await _context.Users
            .AnyAsync(u => string.Equals(u.Email, email, StringComparison.CurrentCultureIgnoreCase));
    }
}
