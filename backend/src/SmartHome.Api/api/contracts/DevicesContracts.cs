namespace SmartHome.Api.Api.Contracts;

/// <summary>
/// Request payload used to register a new physical device.
/// </summary>
public sealed record RegisterDeviceRequest
{
    /// <summary>
    /// Unique external identifier emitted by the device hardware.
    /// </summary>
    public string ExternalId { get; init; } = string.Empty;

    /// <summary>
    /// Friendly display name for the device.
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Sensor type classification used by telemetry processing.
    /// </summary>
    public string SensorType { get; init; } = string.Empty;
}

/// <summary>
/// Response payload returned after successful device registration.
/// </summary>
public sealed record DeviceResponse(Guid Id, string ExternalId, string Name, string SensorType, DateTime RegisteredAtUtc);

/// <summary>
/// Latest telemetry projection returned for a registered device.
/// </summary>
public sealed record DeviceLatestTelemetryResponse(
    Guid DeviceId,
    string ExternalId,
    string Name,
    string SensorType,
    string? LatestMetricType,
    decimal? LatestMetricValue,
    DateTime? LatestEventTimeUtc,
    string Status);
