using SmiTo.Application.DTOs;
using SmiTo.Application.Interfaces;
using SmiTo.Domain.Entities;
using SmiTo.Domain.Repositories;

namespace SmiTo.Application.Services;

public class VisitService : IVisitService
{
    private readonly IUnitOfWork _unitOfWork;

    public VisitService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GeneralResult> TrackVisitAndRedirectAsync(string shortCode, string visitorIp, string userAgent, string? referrer = null)
    {
        var url = await _unitOfWork.URLRepository.GetByShortCodeAsync(shortCode);

        if (url == null)
        {
            return GeneralResult.Failure(new List<string> { "Short URL not found." }, "URL not found");
        }
        if (url.ExpiresAt.HasValue && url.ExpiresAt.Value < DateTime.UtcNow)
        {
            return GeneralResult.Failure(new List<string> { "This short URL has expired." }, "URL expired");
        }

        var deviceInfo = ParseUserAgent(userAgent);

        var country = await GetCountryFromIpAsync(visitorIp);

        var visit = new Visit
        {
            URLId = url.Id,
            VisitorIp = visitorIp,
            UserAgent = userAgent,
            DeviceType = deviceInfo.DeviceType,
            Browser = deviceInfo.Browser,
            Referrer = referrer,
            Country = country,
            VisitedAt = DateTime.UtcNow
        };

        await _unitOfWork.VisitRepository.CreateAsync(visit);

        url.ClickCount++;
        url.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.URLRepository.UpdateAsync(url);
        await _unitOfWork.SaveChangesAsync();

        return GeneralResult<String>.SuccessResult(url.OriginalUrl, "Visit tracked successfully");
    }

    public async Task<GeneralResult> GetVisitsByUrlAsync(Guid urlId, Guid userId, int page = 1, int pageSize = 10)
    {
        var url = await _unitOfWork.URLRepository.GetByIdAsync(urlId);
        if (url == null || url.UserId != userId)
        {
            return GeneralResult.Failure(new List<string> { "URL not found or access denied." }, "Access denied");
        }

        var visits = await _unitOfWork.VisitRepository.GetByUrlIdAsync(urlId, page, pageSize);
        var visitResponses = visits.Select(MapVisitToResponse).ToList();

        return GeneralResult<List<VisitResponse>>.SuccessResult(visitResponses, "Visits retrieved successfully");
    }

    public async Task<GeneralResult> GetVisitStatsAsync(Guid urlId, Guid userId, DateTime? from = null, DateTime? to = null)
    {
        var url = await _unitOfWork.URLRepository.GetByIdAsync(urlId);
        if (url == null || url.UserId != userId)
        {
            return GeneralResult.Failure(new List<string> { "URL not found or access denied." }, "Access denied");
        }

        from ??= DateTime.UtcNow.AddDays(-30);
        to ??= DateTime.UtcNow;

        var dailyStats = await _unitOfWork.VisitRepository.GetVisitStatsByUrlIdAsync(urlId, from.Value, to.Value);
        var totalClicks = await _unitOfWork.VisitRepository.GetTotalVisitCountByUrlIdAsync(urlId);
        var uniqueVisitors = await _unitOfWork.VisitRepository.GetUniqueVisitorCountByUrlIdAsync(urlId, from.Value, to.Value);

        var stats = new VisitStatsResponse
        {
            UrlId = urlId,
            TotalClicks = totalClicks,
            UniqueVisitors = uniqueVisitors,
            DailyStats = (IEnumerable<DTOs.DailyVisitStats>)dailyStats,
            DateRange = new DateRangeResponse
            {
                From = from.Value,
                To = to.Value
            }
        };

        return GeneralResult<VisitStatsResponse>.SuccessResult(stats, "Visit statistics retrieved successfully");
    }

    private static DeviceInfo ParseUserAgent(string userAgent)
    {
        if (string.IsNullOrEmpty(userAgent))
        {
            return new DeviceInfo { DeviceType = "Unknown", Browser = "Unknown" };
        }

        var deviceType = "Desktop";
        var browser = "Unknown";

        userAgent = userAgent.ToLower();

        if (userAgent.Contains("mobile") || userAgent.Contains("android") || userAgent.Contains("iphone"))
            deviceType = "Mobile";
        else if (userAgent.Contains("tablet") || userAgent.Contains("ipad"))
            deviceType = "Tablet";

        if (userAgent.Contains("chrome") && !userAgent.Contains("edg"))
            browser = "Chrome";
        else if (userAgent.Contains("firefox"))
            browser = "Firefox";
        else if (userAgent.Contains("safari") && !userAgent.Contains("chrome"))
            browser = "Safari";
        else if (userAgent.Contains("edg"))
            browser = "Edge";
        else if (userAgent.Contains("opera"))
            browser = "Opera";

        return new DeviceInfo { DeviceType = deviceType, Browser = browser };
    }

    private static async Task<string?> GetCountryFromIpAsync(string ipAddress)
    {
        try
        {
            if (ipAddress == "127.0.0.1" || ipAddress == "::1")
                return "Local";

            return null;
        }
        catch
        {
            return null;
        }
    }

    private static VisitResponse MapVisitToResponse(Visit visit) => new(
        visit.Id,
        visit.VisitedAt,
        visit.VisitorIp,
        visit.UserAgent,
        visit.DeviceType,
        visit.Browser,
        visit.Referrer,
        visit.Country
    );

    private class DeviceInfo
    {
        public string DeviceType { get; set; } = string.Empty;
        public string Browser { get; set; } = string.Empty;
    }
}
