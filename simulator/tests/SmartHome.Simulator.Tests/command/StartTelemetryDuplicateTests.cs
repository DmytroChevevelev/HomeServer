using System.Net;
using SmartHome.Simulator.Commands;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Command;

public sealed class StartTelemetryDuplicateTests
{
    [Fact]
    public async Task HandleStartAsync_SecondStartDoesNotCreateDuplicateLoop()
    {
        var profilePath = CreateProfileFile();
        var profile = new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            1,
            profilePath);

        var backend = new SimulatorBackendClient(new HttpClient(new AcceptedHandler()));
        var publisher = new TelemetryPublisher(backend);
        var handlers = new TelemetryControlCommandHandlers(publisher, new TelemetryProfileLoader());

        var output = new StringWriter();
        var firstState = await handlers.HandleStartAsync(profile, TelemetrySessionState.Idle, output, CancellationToken.None);
        var secondState = await handlers.HandleStartAsync(profile, firstState, output, CancellationToken.None);

        Assert.Equal(TelemetrySessionState.Sending, secondState);
        Assert.Contains("already", output.ToString(), StringComparison.OrdinalIgnoreCase);

        await handlers.HandleStopAsync(secondState, output);
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