using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.DTOs;

public class DetailedURLAnalyticsResponse
{
    public Guid Id { get; set; }
    public string ShortCode { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public int TotalClicks { get; set; }
    public IEnumerable<DailyVisitStats> DailyStats { get; set; } = new List<DailyVisitStats>();
    public IEnumerable<VisitResponse> RecentVisits { get; set; } = new List<VisitResponse>();
    public DeviceStatsResponse DeviceStats { get; set; } = new();

    public DetailedURLAnalyticsResponse() { }

    public DetailedURLAnalyticsResponse(
        Guid id,
        string shortCode,
        string originalUrl,
        int totalClicks,
        IEnumerable<DailyVisitStats> dailyStats,
        IEnumerable<VisitResponse> recentVisits,
        DeviceStatsResponse deviceStats)
    {
        Id = id;
        ShortCode = shortCode;
        OriginalUrl = originalUrl;
        TotalClicks = totalClicks;
        DailyStats = dailyStats;
        RecentVisits = recentVisits;
        DeviceStats = deviceStats;
    }
}