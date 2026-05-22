using SmartHome.Api.Api.Contracts;
using SmartHome.Api.Api.Errors;
using SmartHome.Api.Infrastructure.Repositories;
using SmartHome.Api.Models;
using SmartHome.Api.Services;

namespace SmartHome.Api.Api.Endpoints;

public static class DevicesEndpoints
{
    public static IEndpointRouteBuilder MapDevicesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/devices").WithTags("Devices");

        group.MapPost("", async (RegisterDeviceRequest request, DeviceRepository devices, ValidationErrorFactory errors, HttpContext context) =>
        {
            if (string.IsNullOrWhiteSpace(request.ExternalId) || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.SensorType))
            {
                return Results.BadRequest(errors.Create("validation_error", "Required fields are missing.", context));
            }

            if (await devices.ExistsByExternalIdAsync(request.ExternalId))
            {
                return Results.Conflict(errors.Create("duplicate_external_id", "Device externalId already exists.", context, "externalId"));
            }

            var entity = new Device
            {
                Id = Guid.NewGuid(),
                ExternalId = request.ExternalId.Trim(),
                Name = request.Name.Trim(),
                SensorType = request.SensorType.Trim(),
                RegisteredAtUtc = DateTime.UtcNow,
                IsEnabled = true
            };

            await devices.AddAsync(entity);
            var response = new DeviceResponse(entity.Id, entity.ExternalId, entity.Name, entity.SensorType, entity.RegisteredAtUtc);
            return Results.Created($"/api/devices/{entity.Id}", response);
        })
        .WithName("registerDevice")
        .WithSummary("Register a device")
        .WithDescription("Registers a new device using external id, name, and sensor type.")
        .Produces<DeviceResponse>(StatusCodes.Status201Created)
        .Produces<ValidationError>(StatusCodes.Status400BadRequest)
        .Produces<ValidationError>(StatusCodes.Status409Conflict);

        group.MapGet("", async (LatestTelemetryQueryService latestService) =>
        {
            var latest = await latestService.GetLatestAsync();
            return Results.Ok(latest);
        })
        .WithName("listDevices")
        .WithSummary("List devices with latest telemetry")
        .WithDescription("Returns registered devices and latest telemetry projections.")
        .Produces<IEnumerable<DeviceLatestTelemetryResponse>>(StatusCodes.Status200OK);

        return app;
    }
}
