namespace SmartHome.Api.Api.Contracts;

/// <summary>
/// Request payload used to ingest one telemetry event.
/// </summary>
public sealed record IngestTelemetryRequest
{
	/// <summary>
	/// External identifier of the registered device that emitted the event.
	/// </summary>
	public string DeviceExternalId { get; init; } = string.Empty;

	/// <summary>
	/// Metric type label, such as temperature or humidity.
	/// </summary>
	public string MetricType { get; init; } = string.Empty;

	/// <summary>
	/// Numeric telemetry value captured by the device.
	/// </summary>
	public decimal MetricValue { get; init; }

	/// <summary>
	/// Timestamp of when the event occurred at the device.
	/// </summary>
	public DateTime EventTimeUtc { get; init; }
}
