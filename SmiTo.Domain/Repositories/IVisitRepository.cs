using SmiTo.Domain.Entities;

namespace SmiTo.Domain.Repositories;

public interface IVisitRepository
{
    Task CreateAsync(Visit visit);
    Task<IEnumerable<Visit>> GetByUrlIdAsync(Guid urlId, int page = 1, int pageSize = 10);
    Task<int> GetTotalVisitCountByUrlIdAsync(Guid urlId);
    Task<Dictionary<string, int>> GetDeviceStatsAsync(Guid urlId, DateTime from, DateTime to);
    Task<Dictionary<string, int>> GetBrowserStatsAsync(Guid urlId, DateTime from, DateTime to);
    Task<IEnumerable<DailyVisitStats>> GetVisitStatsByUrlIdAsync(Guid urlId, DateTime from, DateTime to);
    Task<int> GetUniqueVisitorCountByUrlIdAsync(Guid urlId, DateTime from, DateTime to);

}
public class DailyVisitStats
{
    public DateTime Date { get; set; }
    public int VisitCount { get; set; }
    public int UniqueVisitors { get; set; } = 0;
}

