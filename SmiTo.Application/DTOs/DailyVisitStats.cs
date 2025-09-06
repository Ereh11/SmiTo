namespace SmiTo.Application.DTOs;

public class DailyVisitStats
{
    public DateTime Date { get; set; }
    public int VisitCount { get; set; }
    public int UniqueVisitors { get; set; } = 0;
}
