namespace SmartHome.Api.Api.Contracts;

public sealed record IngestTelemetryRequest(string DeviceExternalId, string MetricType, decimal MetricValue, DateTime EventTimeUtc);
