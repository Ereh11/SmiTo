using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.DTOs;

public class VisitStatsResponse
{
    public Guid UrlId { get; set; }
    public int TotalClicks { get; set; }
    public int UniqueVisitors { get; set; }
    public IEnumerable<DailyVisitStats> DailyStats { get; set; } = new List<DailyVisitStats>();
    public DateRangeResponse DateRange { get; set; } = new();
}
