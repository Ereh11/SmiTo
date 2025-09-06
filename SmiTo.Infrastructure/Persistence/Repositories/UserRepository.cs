using Microsoft.EntityFrameworkCore;
using SmiTo.Domain.Repositories;
using SmiTo.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SmiToDbContext _context;
    public UserRepository(SmiToDbContext context)
    {
        _context = context;
    }
    public async Task<bool> IsEmailExist(string email)
    {
        return await _context.Users
            .AnyAsync(u => string.Equals(u.Email, email, StringComparison.CurrentCultureIgnoreCase));
    }
}
