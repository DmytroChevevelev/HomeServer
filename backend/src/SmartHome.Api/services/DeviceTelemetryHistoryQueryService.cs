using SmartHome.Api.Api.Contracts;
using SmartHome.Api.Infrastructure.Repositories;

namespace SmartHome.Api.Services;

/// <summary>
/// Retrieves telemetry history rows for a selected device.
/// </summary>
public sealed class DeviceTelemetryHistoryQueryService(TelemetryRepository telemetry)
{
    public async Task<IReadOnlyList<DeviceTelemetryListItemResponse>> GetForDeviceAsync(
        Guid deviceId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        CancellationToken ct = default)
    {
        var readings = await telemetry.ListForDeviceAsync(deviceId, fromUtc, toUtc, limit, ct);

        return readings
            .Select(reading => new DeviceTelemetryListItemResponse(
                reading.DeviceId,
                reading.MetricType,
                reading.MetricValue,
                reading.EventTimeUtc))
            .ToList();
    }
}
