using SmiTo.Application.DTOs;

namespace SmiTo.Application.Interfaces;

public interface IURLService
{
    Task<GeneralResult> CreateAsync(CreateURLRequest request, Guid userId);
    Task<URLResponse?> GetByIdAsync(Guid id, Guid userId);
    Task<GeneralResult> GetByShortCodeAsync(string shortCode);
    Task<URLListResponse> GetByUserIdAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<URLAnalyticsResponse?> GetAnalyticsAsync(Guid urlId, Guid userId, DateTime? from = null, DateTime? to = null);
}
