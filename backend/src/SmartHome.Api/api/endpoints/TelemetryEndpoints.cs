using SmartHome.Api.Api.Contracts;
using SmartHome.Api.Api.Errors;
using SmartHome.Api.Infrastructure.Repositories;
using SmartHome.Api.Services;

namespace SmartHome.Api.Api.Endpoints;

public static class TelemetryEndpoints
{
    public static IEndpointRouteBuilder MapTelemetryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/telemetry").WithTags("Telemetry");

        group.MapPost("", async (
            IngestTelemetryRequest request,
            DeviceRepository devices,
            TelemetryRepository telemetry,
            TelemetryIngestionService ingestion,
            ValidationErrorFactory errors,
            HttpContext context) =>
        {
            if (string.IsNullOrWhiteSpace(request.DeviceExternalId) || string.IsNullOrWhiteSpace(request.MetricType))
            {
                return Results.BadRequest(errors.Create("validation_error", "Required fields are missing.", context));
            }

            var device = await devices.GetByExternalIdAsync(request.DeviceExternalId);
            if (device is null)
            {
                return Results.NotFound(errors.Create("device_not_found", "Device does not exist.", context, "deviceExternalId"));
            }

            var entity = ingestion.ToEntity(device.Id, request);
            await telemetry.AddAsync(entity);
            return Results.Accepted();
        })
        .WithName("ingestTelemetry")
        .WithSummary("Ingest a telemetry event")
        .WithDescription("Accepts telemetry payloads for registered devices.")
        .Produces(StatusCodes.Status202Accepted)
        .Produces<ValidationError>(StatusCodes.Status400BadRequest)
        .Produces<ValidationError>(StatusCodes.Status404NotFound)
        .Produces<ValidationError>(StatusCodes.Status429TooManyRequests);

        group.MapGet("/latest", async (LatestTelemetryQueryService latestService) =>
        {
            var latest = await latestService.GetLatestAsync();
            return Results.Ok(latest);
        })
        .WithName("getLatestTelemetry")
        .WithSummary("Get latest telemetry by device")
        .WithDescription("Returns latest telemetry projection for each registered device.")
        .Produces<IEnumerable<DeviceLatestTelemetryResponse>>(StatusCodes.Status200OK);

        return app;
    }
}
