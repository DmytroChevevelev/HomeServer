using System.Text.Json;

namespace SmartHome.Simulator.Runtime;

public sealed class TelemetryProfileLoader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public string ResolveProfilePath(string baseDirectory, string deviceName, string profileDirectory = "profiles")
    {
        if (string.IsNullOrWhiteSpace(deviceName))
        {
            throw new InvalidOperationException("Device name is required to resolve telemetry profile path.");
        }

        return Path.Combine(baseDirectory, profileDirectory, $"{deviceName.Trim()}.json");
    }

    public TelemetryProfile Load(string profilePath, string sourceDeviceName)
    {
        if (!File.Exists(profilePath))
        {
            throw new FileNotFoundException($"Telemetry profile file was not found: {profilePath}");
        }

        var json = File.ReadAllText(profilePath);
        TelemetryProfileDocument? doc;

        try
        {
            doc = JsonSerializer.Deserialize<TelemetryProfileDocument>(json, SerializerOptions);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException($"Telemetry profile file is invalid JSON: {profilePath}", ex);
        }

        if (doc?.Metrics is null || doc.Metrics.Count == 0)
        {
            throw new InvalidOperationException("Telemetry profile must contain at least one metric entry.");
        }

        var metrics = new List<TelemetryMetricEntry>(doc.Metrics.Count);
        foreach (var metric in doc.Metrics)
        {
            if (string.IsNullOrWhiteSpace(metric.MetricType))
            {
                throw new InvalidOperationException("Telemetry metric entry must define metricType.");
            }

            metrics.Add(new TelemetryMetricEntry(metric.MetricType.Trim(), metric.MetricValue, metric.EventTimeUtc));
        }

        return new TelemetryProfile(metrics, sourceDeviceName, DateTime.UtcNow);
    }

    public TelemetryProfile LoadForDevice(SimulatorDeviceProfile profile)
    {
        return Load(profile.TelemetryProfilePath, profile.DeviceName);
    }

    private sealed record TelemetryProfileDocument(List<TelemetryMetricDocument> Metrics);

    private sealed record TelemetryMetricDocument(string MetricType, decimal MetricValue, DateTime? EventTimeUtc);
}