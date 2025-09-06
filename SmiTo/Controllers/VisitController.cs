using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmiTo.Application.Interfaces;
using System.Security.Claims;

namespace SmiTo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VisitController : ControllerBase
{
    private readonly IVisitService _visitService;

    public VisitController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet("{urlId}/visits")]
    public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> GetVisits(
        Guid urlId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var userId = GetCurrentUserId();

        var result = await _visitService.GetVisitsByUrlAsync(urlId, userId.Value, page, pageSize);

        if (!result.Success)
            return TypedResults.BadRequest(result);

        return TypedResults.Ok(result);
    }

    [HttpGet("{urlId}/stats")]
    public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> GetVisitStats(
        Guid urlId,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null)
    {
        var userId = GetCurrentUserId();

        var result = await _visitService.GetVisitStatsAsync(urlId, userId.Value, from, to);

        if (!result.Success)
            return TypedResults.BadRequest(result);

        return TypedResults.Ok(result);
    }

    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
