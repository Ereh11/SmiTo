using SmiTo.Domain.Entities;

namespace SmiTo.Domain.Repositories;

public interface IURLRepository
{
    Task<URL?> GetByIdAsync(Guid id);
    Task<URL?> GetByShortCodeAsync(string shortCode);
    Task<IEnumerable<URL>> GetByUserIdAsync(Guid userId, int page, int pageSize);
    Task<URL> CreateAsync(URL url);
    Task<bool> ShortCodeExistsAsync(string shortCode);
    Task<int> GetTotalCountByUserIdAsync(Guid userId);
}
