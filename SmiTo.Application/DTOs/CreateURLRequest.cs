namespace SmiTo.Application.DTOs;

public record CreateURLRequest(
    string OriginalUrl,
    DateTime? ExpiresAt = null
);
