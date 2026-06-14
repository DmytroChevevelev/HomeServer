using System.Net;
using System.Net.Http.Json;
using System.Linq;
using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Devices;

public class DeviceUnregisterTests
{
    [Fact]
    public async Task Delete_Device_Returns_Ok_For_Existing_Device()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var registerPayload = new
        {
            externalId = "ext-unregister-1",
            name = "Garage Sensor",
            sensorType = "temperature"
        };

        var registerResponse = await client.PostAsJsonAsync("/api/devices", registerPayload);
        registerResponse.EnsureSuccessStatusCode();

        using var registerDocument = JsonDocument.Parse(await registerResponse.Content.ReadAsStringAsync());
        var deviceId = registerDocument.RootElement.GetProperty("id").GetGuid();

        var deleteResponse = await client.DeleteAsync($"/api/devices/{deviceId}");

        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        var listResponse = await client.GetAsync("/api/devices");
        listResponse.EnsureSuccessStatusCode();

        using var listDocument = JsonDocument.Parse(await listResponse.Content.ReadAsStringAsync());
        var hasDevice = listDocument.RootElement.EnumerateArray().Any(item =>
            item.TryGetProperty("deviceId", out var value) && value.GetGuid() == deviceId);

        Assert.False(hasDevice);
    }

    [Fact]
    public async Task Delete_Device_Returns_NotFound_For_Unknown_Device()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var response = await client.DeleteAsync($"/api/devices/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
