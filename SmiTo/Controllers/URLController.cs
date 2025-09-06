using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SmiTo.Application.DTOs;
using SmiTo.Application.Interfaces;

namespace SmiTo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class URLController : ControllerBase
    {
        private readonly IURLService _urlService;
        public URLController(IURLService urlService)
        {
            _urlService = urlService;
        }
        [HttpPost]
        public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> CreateURL([FromBody] CreateURLRequest request, [FromBody] Guid UserId)
        {
            var response = await _urlService.CreateAsync(request, UserId);
            if (response.Success)
                return TypedResults.Ok(response);
            return TypedResults.BadRequest(response);
        }
        [HttpGet]
        public async Task<Results<Ok<GeneralResult>, NotFound>> GetOriginalUrl([FromQuery] string ShortUrl)
        {
            var response = await _urlService.GetByShortCodeAsync(ShortUrl);
            if (response.Success)
                return TypedResults.Ok(response);
            return TypedResults.NotFound();
        }
    }
}
