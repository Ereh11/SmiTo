namespace SmiTo.Application.DTOs;

public record URLResponse(
    Guid Id,
    string OriginalUrl,
    string ShortCode,
    string ShortenedUrl,
    DateTime CreatedAt,
    DateTime? ExpiresAt,
    int ClickCount
);
