using Microsoft.AspNetCore.OpenApi;
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

        group.MapPost("", async (RegisterDeviceRequest request, DeviceRepository devices, ValidationErrorFactory errors, ILoggerFactory loggerFactory, HttpContext context) =>
        {
            var logger = loggerFactory.CreateLogger("Devices.Register");
            if (string.IsNullOrWhiteSpace(request.ExternalId) || string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.SensorType))
            {
                logger.LogWarning("Registration rejected due to missing required fields.");
                return Results.BadRequest(errors.Create("validation_error", "Required fields are missing.", context));
            }

            if (await devices.ExistsByExternalIdAsync(request.ExternalId))
            {
                logger.LogWarning("Registration rejected because externalId {ExternalId} already exists.", request.ExternalId);
                return Results.Conflict(errors.Create("duplicate_external_id", "Device externalId already exists.", context, "externalId"));
            }

            var entity = new Device
            {
                Id = Guid.NewGuid(),
                ExternalId = request.ExternalId.Trim(),
                Name = request.Name.Trim(),
                SensorType = request.SensorType.Trim(),
                RegisteredAtUtc = DateTime.UtcNow,
                IsEnabled = request.IsEnabled ?? true
            };

            await devices.AddAsync(entity);
            logger.LogInformation("Device {DeviceId} registered with externalId {ExternalId}.", entity.Id, entity.ExternalId);
            var response = new DeviceResponse(entity.Id, entity.ExternalId, entity.Name, entity.SensorType, entity.RegisteredAtUtc);
            return Results.Created($"/api/devices/{entity.Id}", response);
        })
        .WithName("registerDevice")
        .WithSummary("Register a device")
        .WithDescription("Registers a new device using external id, name, and sensor type.")
        .WithOpenApi(operation =>
        {
            operation.RequestBody ??= new Microsoft.OpenApi.Models.OpenApiRequestBody();
            operation.RequestBody.Description = "Device registration payload containing external id, user-visible name, and sensor type.";
            operation.Responses["201"].Description = "Device registered successfully.";
            operation.Responses["400"].Description = "Required request fields are missing or invalid.";
            operation.Responses["409"].Description = "A device with the same external id already exists.";
            return operation;
        })
        .Produces<DeviceResponse>(StatusCodes.Status201Created)
        .Produces<ValidationError>(StatusCodes.Status400BadRequest)
        .Produces<ValidationError>(StatusCodes.Status409Conflict);

        group.MapGet("", async (LatestTelemetryQueryService latestService, ILoggerFactory loggerFactory) =>
        {
            var latest = await latestService.GetLatestAsync();
            var logger = loggerFactory.CreateLogger("Devices.List");
            logger.LogInformation("Projected {Count} devices with latest telemetry.", latest.Count);
            return Results.Ok(latest);
        })
        .WithName("listDevices")
        .WithSummary("List devices with latest telemetry")
        .WithDescription("Returns registered devices and latest telemetry projections. Browser clients must call from origins allowed by configured CORS policy.")
        .WithOpenApi(operation =>
        {
            operation.Responses["200"].Description = "Device list with latest telemetry projection was returned.";
            operation.Responses["200"].Headers ??= new Dictionary<string, Microsoft.OpenApi.Models.OpenApiHeader>();
            operation.Responses["200"].Headers["Access-Control-Allow-Origin"] = new Microsoft.OpenApi.Models.OpenApiHeader
            {
                Description = "Present for browser requests from origins configured in CORS allowlist."
            };
            return operation;
        })
        .Produces<IEnumerable<DeviceLatestTelemetryResponse>>(StatusCodes.Status200OK);

        group.MapGet("/{deviceId:guid}/telemetry", async (
            Guid deviceId,
            int? limit,
            DeviceRepository devices,
            DeviceTelemetryHistoryQueryService history,
            ValidationErrorFactory errors,
            ILoggerFactory loggerFactory,
            HttpContext context) =>
        {
            var logger = loggerFactory.CreateLogger("Devices.TelemetryList");

            if (limit is <= 0)
            {
                logger.LogWarning("Telemetry list rejected for device {DeviceId} due to invalid limit {Limit}.", deviceId, limit);
                return Results.BadRequest(errors.Create("validation_error", "Limit must be greater than zero.", context, "limit"));
            }

            var existing = await devices.GetByIdAsync(deviceId);
            if (existing is null)
            {
                logger.LogWarning("Telemetry list rejected because device {DeviceId} was not found.", deviceId);
                return Results.NotFound(errors.Create("device_not_found", "Device does not exist.", context, "deviceId"));
            }

            var safeLimit = limit ?? 100;
            var telemetry = await history.GetForDeviceAsync(deviceId, null, null, safeLimit);
            logger.LogInformation("Returned {Count} telemetry rows for device {DeviceId} with limit {Limit}.", telemetry.Count, deviceId, safeLimit);
            return Results.Ok(telemetry);
        })
        .WithName("listDeviceTelemetry")
        .WithSummary("List telemetry for selected device")
        .WithDescription("Returns the latest telemetry records for a selected device. Defaults to the latest 100 records when limit is not provided.")
        .WithOpenApi(operation =>
        {
            var deviceIdParameter = operation.Parameters?.FirstOrDefault(parameter =>
                string.Equals(parameter.Name, "deviceId", StringComparison.OrdinalIgnoreCase));

            if (deviceIdParameter is not null)
            {
                deviceIdParameter.Description = "Unique identifier of the device whose telemetry is requested.";
            }

            var limitParameter = operation.Parameters?.FirstOrDefault(parameter =>
                string.Equals(parameter.Name, "limit", StringComparison.OrdinalIgnoreCase));

            if (limitParameter is not null)
            {
                limitParameter.Description = "Maximum number of newest telemetry records to return. Defaults to 100 and supports up to 500.";
            }

            operation.Responses["200"].Description = "Telemetry rows for the selected device were returned in newest-first order.";
            operation.Responses["400"].Description = "Limit query parameter is invalid.";
            operation.Responses["404"].Description = "Selected device was not found.";
            return operation;
        })
        .Produces<IEnumerable<DeviceTelemetryListItemResponse>>(StatusCodes.Status200OK)
        .Produces<ValidationError>(StatusCodes.Status400BadRequest)
        .Produces<ValidationError>(StatusCodes.Status404NotFound);

        group.MapDelete("/{deviceId:guid}", async (Guid deviceId, DeviceRepository devices, ValidationErrorFactory errors, ILoggerFactory loggerFactory, HttpContext context) =>
        {
            var logger = loggerFactory.CreateLogger("Devices.Unregister");
            var existing = await devices.GetByIdAsync(deviceId);
            if (existing is null)
            {
                logger.LogWarning("Unregister failed because device {DeviceId} was not found.", deviceId);
                return Results.NotFound(errors.Create("device_not_found", "Device does not exist.", context, "deviceId"));
            }

            await devices.DeleteAsync(existing);
            logger.LogInformation("Device {DeviceId} (externalId {ExternalId}) was unregistered with related telemetry removed.", existing.Id, existing.ExternalId);
            return Results.Ok(new DeviceUnregisterResponse(existing.Id, true, "Device unregistered successfully."));
        })
        .WithName("unregisterDevice")
        .WithSummary("Unregister a device")
        .WithDescription("Removes a registered device by its identifier.")
        .WithOpenApi(operation =>
        {
            var deviceIdParameter = operation.Parameters?.FirstOrDefault(parameter =>
                string.Equals(parameter.Name, "deviceId", StringComparison.OrdinalIgnoreCase));

            if (deviceIdParameter is not null)
            {
                deviceIdParameter.Description = "Unique identifier of the device to remove.";
            }

            operation.Responses["200"].Description = "Device was removed successfully.";
            operation.Responses["404"].Description = "Device was not found.";
            return operation;
        })
        .Produces<DeviceUnregisterResponse>(StatusCodes.Status200OK)
        .Produces<ValidationError>(StatusCodes.Status404NotFound);

        return app;
    }
}
