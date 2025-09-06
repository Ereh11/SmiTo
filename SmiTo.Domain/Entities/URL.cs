using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Domain.Entities;

public class URL
{
    public Guid Id { get; set; }
    public string OriginalUrl { get; set; } = null!;
    public string ShortCode { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int ClickCount { get; set; }
    public string ShortenedUrl => $"https://smi.to/{ShortCode}";


    // Navigation property
    public User User { get; set; } = null!;
    public Guid UserId { get; set; }

}
