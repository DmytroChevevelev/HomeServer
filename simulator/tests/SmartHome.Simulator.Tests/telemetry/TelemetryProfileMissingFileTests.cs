using SmartHome.Simulator.Commands;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Telemetry;

public sealed class TelemetryProfileMissingFileTests
{
    [Fact]
    public async Task HandleStartAsync_ReturnsFaulted_WhenProfileMissing()
    {
        var profile = new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            2,
            Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "device-001.json"));

        var handlers = new TelemetryControlCommandHandlers(
            new TelemetryPublisher(new SimulatorBackendClient(new HttpClient(new NoopHandler()))),
            new TelemetryProfileLoader());

        var output = new StringWriter();
        var state = await handlers.HandleStartAsync(profile, TelemetrySessionState.Idle, output, CancellationToken.None);

        Assert.Equal(TelemetrySessionState.Faulted, state);
        Assert.Contains("profile error", output.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private sealed class NoopHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.Accepted));
        }
    }
}