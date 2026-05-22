using System.Net;
using SmartHome.Simulator.Commands;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Command;

public sealed class RegisterDeviceCommandErrorTests
{
    [Fact]
    public async Task ExecuteAsync_ReturnsFalse_ForConflictResponse()
    {
        var client = new HttpClient(new StubHttpMessageHandler(_ => new HttpResponseMessage(HttpStatusCode.Conflict)
        {
            Content = new StringContent("duplicate")
        }));

        var backend = new SimulatorBackendClient(client);
        var handler = new RegisterDeviceCommandHandler(backend);
        var profile = new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            2,
            "profiles/device-001.json");

        var writer = new StringWriter();
        var result = await handler.ExecuteAsync(profile, writer, CancellationToken.None);

        Assert.False(result);
        Assert.Contains("409", writer.ToString());
    }

    [Fact]
    public async Task ExecuteAsync_ReturnsFalse_WhenBackendUnavailable()
    {
        var client = new HttpClient(new ThrowingHttpMessageHandler());

        var backend = new SimulatorBackendClient(client);
        var handler = new RegisterDeviceCommandHandler(backend);
        var profile = new SimulatorDeviceProfile(
            "http://localhost:5151/api",
            "device-001",
            "device-001",
            "temperature",
            2,
            "profiles/device-001.json");

        var writer = new StringWriter();
        var result = await handler.ExecuteAsync(profile, writer, CancellationToken.None);

        Assert.False(result);
        Assert.Contains("failed to reach backend", writer.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(respond(request));
        }
    }

    private sealed class ThrowingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new HttpRequestException("backend unavailable");
        }
    }
}