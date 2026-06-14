using SmartHome.Simulator.Commands;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Telemetry;

public sealed class TelemetryProfileInvalidFileTests
{
    [Fact]
    public async Task HandleStartAsync_ReturnsFaulted_WhenProfileMalformed()
    {
        var dir = Path.Combine(Path.GetTempPath(), "simulator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "device-001.json");
        File.WriteAllText(path, "{ invalid-json }");

        var profile = new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            2,
            path);

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