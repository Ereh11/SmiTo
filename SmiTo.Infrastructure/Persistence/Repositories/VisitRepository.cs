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

    public async Task CreateAsync(Visit visit)
    {
        await _context.Visits.AddAsync(visit);
    }

    public async Task<Dictionary<string, int>> GetBrowserStatsAsync(Guid urlId, DateTime from, DateTime to)
    {
        return await _context.Visits
            .Where(v => v.URLId == urlId && v.VisitedAt >= from && v.VisitedAt <= to)
            .GroupBy(v => v.Browser)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
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
        return await _context.Visits
            .CountAsync(v => v.URLId == urlId);
    }

    public async Task<Dictionary<string, int>> GetDeviceStatsAsync(Guid urlId, DateTime from, DateTime to)
    {
        return await _context.Visits
            .Where(v => v.URLId == urlId && v.VisitedAt >= from && v.VisitedAt <= to)
            .GroupBy(v => v.DeviceType)
            .ToDictionaryAsync(g => g.Key, g => g.Count());
    }

    public async Task<IEnumerable<DailyVisitStats>> GetVisitStatsByUrlIdAsync(Guid urlId, DateTime from, DateTime to)
    {
        return await _context.Visits
            .Where(v => v.URLId == urlId && v.VisitedAt >= from && v.VisitedAt <= to)
            .GroupBy(v => v.VisitedAt.Date)
            .Select(g => new DailyVisitStats
            {
                Date = g.Key,
                VisitCount = g.Count(),
                UniqueVisitors = g.Select(v => v.VisitorIp).Distinct().Count()
            })
            .OrderBy(d => d.Date)
            .ToListAsync();
    }

    public async Task<int> GetUniqueVisitorCountByUrlIdAsync(Guid urlId, DateTime from, DateTime to)
    {
        return await _context.Visits
            .Where(v => v.URLId == urlId && v.VisitedAt >= from && v.VisitedAt <= to)
            .Select(v => v.VisitorIp)
            .Distinct()
            .CountAsync();
    }
}
