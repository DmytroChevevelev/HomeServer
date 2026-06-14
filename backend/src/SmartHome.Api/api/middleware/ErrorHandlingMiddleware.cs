using SmartHome.Api.Api.Errors;

namespace SmartHome.Api.Api.Middleware;

public sealed class ErrorHandlingMiddleware(RequestDelegate next, ValidationErrorFactory errors)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var payload = errors.Create("unhandled_error", ex.Message, context);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(payload);
        }
    }
}
