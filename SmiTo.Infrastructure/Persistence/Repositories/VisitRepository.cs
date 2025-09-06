using Microsoft.EntityFrameworkCore;
using SmiTo.Domain.Entities;
using SmiTo.Domain.Repositories;
using SmiTo.Infrastructure.Persistence.Context;

namespace SmiTo.Infrastructure.Persistence.Repositories;

public class VisitRepository : IVisitRepository
{
    private readonly SmiToDbContext _context;

    public VisitRepository(SmiToDbContext context)
    {
        _context = context;
    }

    public async Task<Visit> CreateAsync(Visit visit)
    {
        _context.Visits.Add(visit);
        await _context.SaveChangesAsync();
        return visit;
    }

    public async Task<IEnumerable<Visit>> GetByUrlIdAsync(Guid urlId, int page, int pageSize)
    {
        return await _context.Visits
            .Where(v => v.URLId == urlId)
            .OrderByDescending(v => v.VisitedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalVisitCountByUrlIdAsync(Guid urlId)
    {
        return await _context.Visits.CountAsync(v => v.URLId == urlId);
    }

    public async Task<Dictionary<DateTime, int>> GetVisitStatsByUrlIdAsync(Guid urlId, DateTime from, DateTime to)
    {
        var visits = await _context.Visits
            .Where(v => v.URLId == urlId && v.VisitedAt >= from && v.VisitedAt <= to)
            .GroupBy(v => v.VisitedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return visits.ToDictionary(x => x.Date, x => x.Count);
    }
}
