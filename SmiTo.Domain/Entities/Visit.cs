using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Domain.Entities;

public class Visit
{
    public Guid Id { get; set; }
    public DateTime VisitedAt { get; set; } = DateTime.UtcNow;
    public string VisitorIp { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
    public string Browser { get; set; } = null!;
    public string? Referrer { get; set; }
    public string? Country { get; set; }
    // Navigation property
    public URL URL { get; set; } = null!;
    public Guid URLId { get; set; }
}
