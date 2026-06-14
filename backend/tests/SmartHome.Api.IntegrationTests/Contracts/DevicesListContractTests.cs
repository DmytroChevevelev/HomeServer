using System.Net;
using System.Net.Http.Json;
using System.Linq;
using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class DevicesListContractTests
{
    [Fact]
    public async Task GetDevices_Returns_Expected_Summary_Fields()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        await client.PostAsJsonAsync("/api/devices", new
        {
            externalId = "ext-contract-1",
            name = "Bedroom Sensor",
            sensorType = "temperature"
        });

        var response = await client.GetAsync("/api/devices");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var first = json.RootElement.EnumerateArray().First();

        Assert.True(first.TryGetProperty("deviceId", out _));
        Assert.True(first.TryGetProperty("externalId", out _));
        Assert.True(first.TryGetProperty("name", out _));
        Assert.True(first.TryGetProperty("sensorType", out _));
        Assert.True(first.TryGetProperty("status", out _));
        Assert.True(first.TryGetProperty("latestMetricType", out _));
        Assert.True(first.TryGetProperty("latestMetricValue", out _));
        Assert.True(first.TryGetProperty("latestEventTimeUtc", out _));
    }
}
