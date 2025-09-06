namespace SmiTo.Application.DTOs;

public record CreateVisitRequest(
    string VisitorIp,
    string UserAgent,
    string? Referrer = null
);
