namespace SmiTo.Application.DTOs;

public record URLAnalyticsResponse(
    Guid Id,
    string ShortCode,
    string OriginalUrl,
    int TotalClicks,
    IEnumerable<DailyVisitStats> DailyStats,
    IEnumerable<VisitResponse> RecentVisits,
    VisitStatsResponse? DetailedStats = null
);
