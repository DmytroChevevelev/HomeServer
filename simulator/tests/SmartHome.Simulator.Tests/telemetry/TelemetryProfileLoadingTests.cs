using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Telemetry;

public sealed class TelemetryProfileLoadingTests
{
    [Fact]
    public void Load_UsesConfiguredDeviceProfilePath()
    {
        var dir = Path.Combine(Path.GetTempPath(), "simulator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "device-001.json");
        File.WriteAllText(path, "{\"metrics\":[{\"metricType\":\"temperature\",\"metricValue\":22.3}]}");

        var profile = new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            2,
            path);

        var loader = new TelemetryProfileLoader();
        var loaded = loader.LoadForDevice(profile);

        Assert.Single(loaded.Metrics);
        Assert.Equal("temperature", loaded.Metrics[0].MetricType);
    }
}