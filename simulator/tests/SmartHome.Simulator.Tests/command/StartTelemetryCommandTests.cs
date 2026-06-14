using System.Net;
using SmartHome.Simulator.Commands;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Command;

public sealed class StartTelemetryCommandTests
{
    [Fact]
    public async Task HandleStartAsync_StartsSingleActiveLoop()
    {
        var profilePath = CreateProfileFile();
        var profile = CreateDeviceProfile(profilePath);
        var backend = new SimulatorBackendClient(new HttpClient(new AcceptedHandler()));
        var publisher = new TelemetryPublisher(backend);
        var handlers = new TelemetryControlCommandHandlers(publisher, new TelemetryProfileLoader());

        var output = new StringWriter();
        var state = await handlers.HandleStartAsync(profile, TelemetrySessionState.Idle, output, CancellationToken.None);

        Assert.Equal(TelemetrySessionState.Sending, state);
        Assert.True(publisher.IsRunning);

        await handlers.HandleStopAsync(state, output);
    }

    private static SimulatorDeviceProfile CreateDeviceProfile(string profilePath)
    {
        return new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            1,
            profilePath);
    }

    private static string CreateProfileFile()
    {
        var dir = Path.Combine(Path.GetTempPath(), "simulator-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(dir);
        var path = Path.Combine(dir, "device-001.json");
        File.WriteAllText(path, "{\"metrics\":[{\"metricType\":\"temperature\",\"metricValue\":22.3}]}");
        return path;
    }

    private sealed class AcceptedHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent("accepted")
            });
        }
    }
}