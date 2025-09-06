using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmiTo.Application.Interfaces;

namespace SmiTo.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RedirectController : ControllerBase
{
    private readonly IVisitService _visitService;

    public RedirectController(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [HttpGet("{shortCode}")]
    public async Task<Results<Ok<GeneralResult>, NotFound<GeneralResult>>> RedirectToOriginalUrl(string shortCode)
    {
        var visitorIp = GetVisitorIp();
        var userAgent = Request.Headers["User-Agent"].ToString();
        var referrer = Request.Headers["Referer"].ToString();

        var result = await _visitService.TrackVisitAndRedirectAsync(
            shortCode,
            visitorIp,
            userAgent,
            string.IsNullOrEmpty(referrer) ? null : referrer
        );

        if (!result.Success)
            return TypedResults.NotFound(result);

        return TypedResults.Ok(result);
    }

    [HttpGet("resolve/{shortCode}")]
    public async Task<Results<Ok<GeneralResult>, NotFound<GeneralResult>>> ResolveShortUrl(string shortCode)
    {
        var visitorIp = GetVisitorIp();
        var userAgent = Request.Headers["User-Agent"].ToString();
        var referrer = Request.Headers["Referer"].ToString();

        var result = await _visitService.TrackVisitAndRedirectAsync(
            shortCode,
            visitorIp,
            userAgent,
            string.IsNullOrEmpty(referrer) ? null : referrer
        );

        if (!result.Success)
            return TypedResults.NotFound(result);

        return TypedResults.Ok(result);
    }

    private string GetVisitorIp()
    {
        var forwardedFor = Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
            return forwardedFor.Split(',').First().Trim();

        var realIp = Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp))
            return realIp;

        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }
}
