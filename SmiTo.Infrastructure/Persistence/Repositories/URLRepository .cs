using Microsoft.EntityFrameworkCore;
using SmiTo.Domain.Entities;
using SmiTo.Domain.Repositories;
using SmiTo.Infrastructure.Persistence.Context;

namespace SmiTo.Infrastructure.Persistence.Repositories;

public class URLRepository : IURLRepository
{
    private readonly SmiToDbContext _context;

    public URLRepository(SmiToDbContext context)
    {
        _context = context;
    }

    public async Task<URL?> GetByIdAsync(Guid id)
    {
        return await _context.URLs
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<URL?> GetByShortCodeAsync(string shortCode)
    {
        return await _context.URLs
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
    }

    public async Task<IEnumerable<URL>> GetByUserIdAsync(Guid userId, int page, int pageSize)
    {
        return await _context.URLs
            .Where(u => u.UserId == userId)
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task CreateAsync(URL url)
    {
        await _context.URLs.AddAsync(url);
    }

    public async Task UpdateAsync(URL url)
    {

    }
    public async Task<bool> ShortCodeExistsAsync(string shortCode)
    {
        return await _context.URLs.AnyAsync(u => u.ShortCode == shortCode);
    }

    public async Task<int> GetTotalCountByUserIdAsync(Guid userId)
    {
        return await _context.URLs.CountAsync(u => u.UserId == userId);
        
    }
}
