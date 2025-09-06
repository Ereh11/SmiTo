using SmiTo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiTo.Application.Interfaces;

public interface IVisitService
{
    Task<GeneralResult> TrackVisitAndRedirectAsync(string shortCode, string visitorIp, string userAgent, string? referrer = null);
    Task<GeneralResult> GetVisitStatsAsync(Guid urlId, Guid userId, DateTime? from = null, DateTime? to = null);
    Task<GeneralResult> GetVisitsByUrlAsync(Guid urlId, Guid userId, int page = 1, int pageSize = 10);
}
