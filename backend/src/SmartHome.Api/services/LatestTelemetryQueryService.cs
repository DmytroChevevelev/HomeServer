using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Api.Contracts;
using SmartHome.Api.Infrastructure.Repositories;

namespace SmartHome.Api.Services;

public sealed class LatestTelemetryQueryService(DeviceRepository devices, TelemetryRepository telemetry, DeviceStatusEvaluator status)
{
    public async Task<List<DeviceLatestTelemetryResponse>> GetLatestAsync(CancellationToken ct = default)
    {
        var deviceList = await devices.Query().ToListAsync(ct);
        var readings = await telemetry.Query().ToListAsync(ct);

        return deviceList
            .Select(device =>
            {
                var latest = readings
                    .Where(r => r.DeviceId == device.Id)
                    .OrderByDescending(r => r.EventTimeUtc)
                    .FirstOrDefault();

                return new DeviceLatestTelemetryResponse(
                    device.Id,
                    device.ExternalId,
                    device.Name,
                    device.SensorType,
                    latest?.MetricType,
                    latest?.MetricValue,
                    latest?.EventTimeUtc,
                    status.Evaluate(latest?.EventTimeUtc));
            })
            .ToList();
    }
}
