using SmartHome.Api.Api.Contracts;
using SmartHome.Api.Models;

namespace SmartHome.Api.Services;

public sealed class TelemetryIngestionService
{
    public TelemetryReading ToEntity(Guid deviceId, IngestTelemetryRequest request)
    {
        return new TelemetryReading
        {
            Id = Guid.NewGuid(),
            DeviceId = deviceId,
            MetricType = request.MetricType,
            MetricValue = request.MetricValue,
            EventTimeUtc = request.EventTimeUtc,
            IngestedAtUtc = DateTime.UtcNow
        };
    }
}
