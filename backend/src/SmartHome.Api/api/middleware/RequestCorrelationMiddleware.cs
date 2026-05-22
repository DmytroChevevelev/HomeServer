using System.Diagnostics;

namespace SmartHome.Api.Api.Middleware;

public sealed class RequestCorrelationMiddleware(RequestDelegate next, ILogger<RequestCorrelationMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        context.TraceIdentifier = Activity.Current?.Id ?? Guid.NewGuid().ToString("N");
        using (logger.BeginScope(new Dictionary<string, object> { ["RequestId"] = context.TraceIdentifier }))
        {
            logger.LogInformation("Request started {Method} {Path}", context.Request.Method, context.Request.Path);
            await next(context);
            logger.LogInformation("Request finished {StatusCode}", context.Response.StatusCode);
        }
    }
}
