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

    /// <summary>
    /// Optional enabled flag applied during registration. Defaults to true when omitted.
    /// </summary>
    public bool? IsEnabled { get; init; }
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

/// <summary>
/// Telemetry history row returned for one selected device.
/// </summary>
/// <param name="DeviceId">Unique identifier of the device.</param>
/// <param name="MetricType">Telemetry metric type, such as temperature or humidity.</param>
/// <param name="MetricValue">Measured metric value for the event.</param>
/// <param name="EventTimeUtc">Timestamp when the telemetry event occurred.</param>
public sealed record DeviceTelemetryListItemResponse(
    Guid DeviceId,
    string MetricType,
    decimal MetricValue,
    DateTime EventTimeUtc);

/// <summary>
/// Response payload returned after a successful device unregister operation.
/// </summary>
/// <param name="DeviceId">Identifier of the removed device.</param>
/// <param name="Success">Indicates whether the operation succeeded.</param>
/// <param name="Message">Human-readable operation result.</param>
public sealed record DeviceUnregisterResponse(Guid DeviceId, bool Success, string Message);
