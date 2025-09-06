namespace SmiTo.Application.DTOs;

public record CreateURLRequest(
    Guid userId,
    string OriginalUrl,
    DateTime? ExpiresAt = null
);
