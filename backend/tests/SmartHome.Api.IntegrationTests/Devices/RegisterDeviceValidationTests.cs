using System.Net;
using System.Net.Http.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Devices;

public class RegisterDeviceValidationTests
{
    [Fact]
    public async Task Invalid_Request_Returns_BadRequest()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var payload = new
        {
            externalId = "",
            name = "",
            sensorType = ""
        };

        var response = await client.PostAsJsonAsync("/api/devices", payload);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
