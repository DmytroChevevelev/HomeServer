using Microsoft.Extensions.Configuration;

namespace SmartHome.Simulator.Runtime;

public sealed class SimulatorConfigurationLoader
{
    public const int DefaultSendIntervalSeconds = 2;

    public SimulatorDeviceProfile Load(IConfiguration config, string baseDirectory)
    {
        var apiBaseUrl = (config["ApiBaseUrl"] ?? "http://localhost:5151/api").Trim();
        ValidateApiBaseUrl(apiBaseUrl);

        var deviceExternalId = RequireNonEmpty(config["DeviceExternalId"], "DeviceExternalId");
        var deviceName = (config["DeviceName"] ?? deviceExternalId).Trim();
        var sensorType = (config["SensorType"] ?? "temperature-sensor").Trim();

        if (string.IsNullOrWhiteSpace(deviceName))
        {
            throw new InvalidOperationException("DeviceName must be configured.");
        }

        if (string.IsNullOrWhiteSpace(sensorType))
        {
            throw new InvalidOperationException("SensorType must be configured.");
        }

        var configuredInterval = int.TryParse(config["SendIntervalSeconds"], out var parsedInterval)
            ? parsedInterval
            : 0;
        var interval = configuredInterval > 0 ? configuredInterval : DefaultSendIntervalSeconds;

        var profileDirectory = (config["TelemetryProfileDirectory"] ?? "profiles").Trim();
        var profilePath = Path.Combine(baseDirectory, profileDirectory, $"{deviceName}.json");

        return new SimulatorDeviceProfile(
            apiBaseUrl,
            deviceExternalId,
            deviceName,
            sensorType,
            interval,
            profilePath);
    }

    private static string RequireNonEmpty(string? value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"{key} must be configured.");
        }

        return value.Trim();
    }

    private static void ValidateApiBaseUrl(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new InvalidOperationException("ApiBaseUrl must be an absolute HTTP/HTTPS URL.");
        }
    }
}