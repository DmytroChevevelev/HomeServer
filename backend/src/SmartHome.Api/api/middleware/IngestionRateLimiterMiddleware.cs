using System.Collections.Concurrent;
using SmartHome.Api.Api.Errors;

namespace SmartHome.Api.Api.Middleware;

public sealed class IngestionRateLimiterMiddleware(RequestDelegate next, IConfiguration config, ValidationErrorFactory errors)
{
    private static readonly ConcurrentQueue<DateTime> Requests = new();

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments("/telemetry", StringComparison.OrdinalIgnoreCase)
            && !context.Request.Path.StartsWithSegments("/api/telemetry", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        var now = DateTime.UtcNow;
        var limit = config.GetValue<int?>("RateLimiting:MaxTelemetryRequestsPerMinute") ?? 3000;

        Requests.Enqueue(now);
        while (Requests.TryPeek(out var old) && old < now.AddMinutes(-1))
        {
            Requests.TryDequeue(out _);
        }

        if (Requests.Count > limit)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsJsonAsync(errors.Create("ingestion_throttled", "Telemetry ingest rate exceeded.", context));
            return;
        }

        await next(context);
    }
}
