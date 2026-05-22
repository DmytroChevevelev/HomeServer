namespace SmartHome.Simulator.Runtime;

public sealed record SimulatorCommand(string Name, IReadOnlyList<string> Arguments, DateTime ReceivedAtUtc);

public sealed record SimulatorDeviceProfile(
    string ApiBaseUrl,
    string DeviceExternalId,
    string DeviceName,
    string SensorType,
    int SendIntervalSeconds,
    string TelemetryProfilePath);

public sealed record TelemetryMetricEntry(string MetricType, decimal MetricValue, DateTime? EventTimeUtc = null);

public sealed record TelemetryProfile(IReadOnlyList<TelemetryMetricEntry> Metrics, string SourceDeviceName, DateTime LoadedAtUtc);