namespace SmiTo.Application.DTOs;

public record VisitResponse(
    Guid Id,
    DateTime VisitedAt,
    string VisitorIp,
    string UserAgent,
    string DeviceType,
    string Browser,
    string? Referrer,
    string? Country
);
