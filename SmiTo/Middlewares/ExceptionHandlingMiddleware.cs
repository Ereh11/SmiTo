using FluentValidation;

namespace SmiTo.Middlewares;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation error: {Errors}", string.Join(", ", ex.Errors.Select(e => e.ErrorMessage)));

            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            var result = GeneralResult.Failure(errors, "Validation failed");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            var result = GeneralResult.Failure(new List<string> { ex.Message }, "Unauthorized");

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var result = GeneralResult.Failure(new List<string> { "An unexpected error occurred." }, "Server error");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
