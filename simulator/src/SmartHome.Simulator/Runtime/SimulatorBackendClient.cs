using System.Net.Http.Json;

namespace SmartHome.Simulator.Runtime;

public sealed class SimulatorBackendClient(HttpClient client)
{
    public Task<HttpResponseMessage> RegisterDeviceAsync(SimulatorDeviceProfile profile, CancellationToken ct = default)
    {
        var payload = new RegisterDevicePayload(profile.DeviceExternalId, profile.DeviceName, profile.SensorType);
        var endpoint = BuildApiUri(profile.ApiBaseUrl, "devices");
        return client.PostAsJsonAsync(endpoint, payload, ct);
    }

    public Task<HttpResponseMessage> SendTelemetryAsync(
        SimulatorDeviceProfile profile,
        TelemetryMetricEntry metric,
        CancellationToken ct = default)
    {
        var payload = new IngestTelemetryPayload(
            profile.DeviceExternalId,
            metric.MetricType,
            metric.MetricValue,
            metric.EventTimeUtc ?? DateTime.UtcNow);

        var endpoint = BuildApiUri(profile.ApiBaseUrl, "telemetry");
        return client.PostAsJsonAsync(endpoint, payload, ct);
    }

    private static Uri BuildApiUri(string baseUrl, string route)
    {
        var root = baseUrl.EndsWith('/') ? baseUrl : $"{baseUrl}/";
        return new Uri(new Uri(root), route);
    }

    private sealed record RegisterDevicePayload(string ExternalId, string Name, string SensorType);

    private sealed record IngestTelemetryPayload(
        string DeviceExternalId,
        string MetricType,
        decimal MetricValue,
        DateTime EventTimeUtc);
}