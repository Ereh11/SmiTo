namespace SmiTo.Application.DTOs;

public record URLListResponse(
    IEnumerable<URLResponse> URLs,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);
