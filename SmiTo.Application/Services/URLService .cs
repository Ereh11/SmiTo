using SmiTo.Application.DTOs;
using SmiTo.Application.Interfaces;
using SmiTo.Domain.Entities;
using SmiTo.Domain.Repositories;

namespace SmiTo.Application.Services;

public class URLService : IURLService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IShortCodeGenerator _shortCodeGenerator;
    public URLService(IUnitOfWork unitOfWork, IShortCodeGenerator shortCodeGenerator)
    {
        _unitOfWork = unitOfWork;
        _shortCodeGenerator = shortCodeGenerator;
    }
    public async Task<GeneralResult> CreateAsync(CreateURLRequest request)
    {
        var userExists = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);
        if (userExists == null)
        {
            return GeneralResult.Failure(new List<string> { "User not found." });
        }
        var shortCode =  await _shortCodeGenerator.GenerateUniqueShortCodeAsync();

        var url = new URL
        {
            OriginalUrl = request.OriginalUrl,
            ShortCode = shortCode,
            UserId = request.userId,
            ExpiresAt = request.ExpiresAt == null 
                ? DateTime.UtcNow + TimeSpan.FromDays(30)
                : request.ExpiresAt
        };

        var createdUrl = await _unitOfWork.URLRepository
            .CreateAsync(url);
        await _unitOfWork.SaveChangesAsync();
        
        return GeneralResult<URLResponse>.SuccessResult(MapToResponse(createdUrl), "Shortened URL created successfully.");
    }

    public async Task<GeneralResult> GetByShortCodeAsync(string shortCode)
    {
        var url = await _unitOfWork.URLRepository
            .GetByShortCodeAsync(shortCode);

        if (url == null)
        { 
            return GeneralResult.Failure(
                new List<string> { "Short code not found." }
                );
        }


        if (url.ExpiresAt.HasValue && url.ExpiresAt.Value < DateTime.UtcNow)
        {
            return GeneralResult.Failure(
                new List<string> { "This short URL has expired." }
                );
        }
        return GeneralResult<URLResponse>.SuccessResult(MapToResponse(url), "Original URL retrieved successfully.");
    }

    public async Task<URLListResponse> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 10)
    {
        var urls = await _unitOfWork.URLRepository.GetByUserIdAsync(userId, page, pageSize);
        var totalCount = await _unitOfWork.URLRepository.GetTotalCountByUserIdAsync(userId);
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new URLListResponse(
            urls.Select(MapToResponse),
            totalCount,
            page,
            pageSize,
            totalPages
        );
    }

    public async Task<URLAnalyticsResponse?> GetAnalyticsAsync(Guid urlId, Guid userId, DateTime? from = null, DateTime? to = null)
    {
        var url = await _unitOfWork.URLRepository.GetByIdAsync(urlId);
        if (url == null || url.UserId != userId)
            return null;

        from ??= DateTime.UtcNow.AddDays(-30);
        to ??= DateTime.UtcNow;

        var dailyStats = await _unitOfWork.VisitRepository.GetVisitStatsByUrlIdAsync(urlId, from.Value, to.Value);
        var recentVisits = await _unitOfWork.VisitRepository.GetByUrlIdAsync(urlId, 1, 10);

        return new URLAnalyticsResponse(
            url.Id,
            url.ShortCode,
            url.OriginalUrl,
            url.ClickCount,
            dailyStats,
            recentVisits.Select(MapVisitToResponse)
        );
    }

    private static URLResponse MapToResponse(URL url) => new(
        url.Id,
        url.OriginalUrl,
        url.ShortCode,
        url.ShortenedUrl,
        url.CreatedAt,
        url.ExpiresAt,
        url.ClickCount
    );

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
}
