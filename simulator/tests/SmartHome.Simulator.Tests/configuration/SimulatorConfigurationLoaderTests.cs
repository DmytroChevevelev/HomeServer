using Microsoft.Extensions.Configuration;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Configuration;

public sealed class SimulatorConfigurationLoaderTests
{
    [Fact]
    public void Load_UsesDefaultInterval_WhenIntervalMissingOrInvalid()
    {
        var config = BuildConfig(new Dictionary<string, string?>
        {
            ["ApiBaseUrl"] = "http://localhost:5151/api",
            ["DeviceExternalId"] = "device-001",
            ["DeviceName"] = "device-001",
            ["SensorType"] = "temperature",
            ["SendIntervalSeconds"] = "0"
        });

        var loader = new SimulatorConfigurationLoader();
        var profile = loader.Load(config, "C:/simulator");

        Assert.Equal(SimulatorConfigurationLoader.DefaultSendIntervalSeconds, profile.SendIntervalSeconds);
    }

    [Fact]
    public void Load_ResolvesProfilePath_ByDeviceNameConvention()
    {
        var config = BuildConfig(new Dictionary<string, string?>
        {
            ["ApiBaseUrl"] = "http://localhost:5151/api",
            ["DeviceExternalId"] = "external-01",
            ["DeviceName"] = "kitchen-thermostat",
            ["SensorType"] = "temperature",
            ["TelemetryProfileDirectory"] = "profiles"
        });

        var loader = new SimulatorConfigurationLoader();
        var profile = loader.Load(config, "C:/simulator");

        Assert.EndsWith(Path.Combine("profiles", "kitchen-thermostat.json"), profile.TelemetryProfilePath);
    }

    [Fact]
    public void Load_Throws_WhenApiBaseUrlInvalid()
    {
        var config = BuildConfig(new Dictionary<string, string?>
        {
            ["ApiBaseUrl"] = "not-a-url",
            ["DeviceExternalId"] = "device-001",
            ["DeviceName"] = "device-001",
            ["SensorType"] = "temperature"
        });

        var loader = new SimulatorConfigurationLoader();

        var ex = Assert.Throws<InvalidOperationException>(() => loader.Load(config, "C:/simulator"));
        Assert.Contains("ApiBaseUrl", ex.Message);
    }

    [Fact]
    public void ProfileLoader_LoadsFixtureMetrics()
    {
        var loader = new TelemetryProfileLoader();
        var fixturePath = Path.Combine(AppContext.BaseDirectory, "fixtures", "device-001.json");

        var profile = loader.Load(fixturePath, "device-001");

        Assert.Equal("device-001", profile.SourceDeviceName);
        Assert.Equal(2, profile.Metrics.Count);
        Assert.Equal("temperature", profile.Metrics[0].MetricType);
    }

    private static IConfiguration BuildConfig(Dictionary<string, string?> values)
    {
        return new ConfigurationBuilder().AddInMemoryCollection(values).Build();
    }
}