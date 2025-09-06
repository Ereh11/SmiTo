namespace SmiTo.Application.DTOs;

public record URLAnalyticsResponse(
    Guid URLId,
    string ShortCode,
    string OriginalUrl,
    int TotalClicks,
    Dictionary<DateTime, int> DailyClicks,
    IEnumerable<VisitResponse> RecentVisits
);
