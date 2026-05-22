namespace SmartHome.Api.Api.Contracts;

public sealed record RegisterDeviceRequest(string ExternalId, string Name, string SensorType);
public sealed record DeviceResponse(Guid Id, string ExternalId, string Name, string SensorType, DateTime RegisteredAtUtc);
public sealed record DeviceLatestTelemetryResponse(
    Guid DeviceId,
    string ExternalId,
    string Name,
    string SensorType,
    string? LatestMetricType,
    decimal? LatestMetricValue,
    DateTime? LatestEventTimeUtc,
    string Status);
