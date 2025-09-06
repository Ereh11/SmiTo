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
        public async Task<Results<Ok<GeneralResult>, BadRequest<GeneralResult>>> CreateURL([FromBody] CreateURLRequest request)
        {
            var response = await _urlService.CreateAsync(request);
            if (response.Success)
                return TypedResults.Ok(response);
            return TypedResults.BadRequest(response);
        }
        [HttpGet("{ShortCode}")]
        public async Task<Results<Ok<GeneralResult>, NotFound<GeneralResult>>> GetOriginalUrl([FromRoute] string ShortCode)
        {
            var response = await _urlService.GetByShortCodeAsync(ShortCode);
            if (response.Success)
                return TypedResults.Ok(response);
            return TypedResults.NotFound(response);
        }
    }
}
