using System.Net;
using System.Text;
using SmartHome.Simulator.Commands;
using SmartHome.Simulator.Runtime;
using Xunit;

namespace SmartHome.Simulator.Tests.Command;

public sealed class RegisterDeviceCommandTests
{
    [Fact]
    public async Task ExecuteAsync_SendsRegisterRequest_AndReportsSuccess()
    {
        HttpRequestMessage? captured = null;
        var client = BuildClient(req =>
        {
            captured = req;
            return new HttpResponseMessage(HttpStatusCode.Created)
            {
                Content = new StringContent("{\"id\":\"device-001\"}", Encoding.UTF8, "application/json")
            };
        });

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

        Assert.True(result);
        Assert.NotNull(captured);
        Assert.Equal(HttpMethod.Post, captured!.Method);
        Assert.Equal("http://localhost:5151/api/devices", captured.RequestUri!.ToString());
        var output = writer.ToString();
        Assert.Contains("success", output, StringComparison.OrdinalIgnoreCase);
    }

    private static HttpClient BuildClient(Func<HttpRequestMessage, HttpResponseMessage> respond)
    {
        var handler = new StubHttpMessageHandler(respond);
        return new HttpClient(handler);
    }

    private sealed class StubHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(respond(request));
        }
    }
}