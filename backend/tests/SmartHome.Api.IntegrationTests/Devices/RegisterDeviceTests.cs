using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Devices;

public class RegisterDeviceTests
{
    [Fact]
    public async Task Registers_Device_Returns_Created_Response()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var payload = new
        {
            externalId = "ext-register-1",
            name = "Kitchen Sensor",
            sensorType = "temperature",
            isEnabled = true
        };

        var response = await client.PostAsJsonAsync("/api/devices", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        using var content = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(payload.externalId, content.RootElement.GetProperty("externalId").GetString());
        Assert.Equal(payload.name, content.RootElement.GetProperty("name").GetString());
        Assert.Equal(payload.sensorType, content.RootElement.GetProperty("sensorType").GetString());
    }
}
