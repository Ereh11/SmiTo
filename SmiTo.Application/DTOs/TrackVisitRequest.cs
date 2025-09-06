using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.DTOs;

public class TrackVisitRequest
{
    public string ShortCode { get; set; } = string.Empty;
    public string? Referrer { get; set; }
}
