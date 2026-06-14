using System.Net;
using System.Net.Http.Json;
using System.Linq;
using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Telemetry;

public class TelemetryProjectionTests
{
    [Fact]
    public async Task Newer_Telemetry_Is_Projected_As_Latest_Value()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var externalId = "ext-telemetry-projection-1";
        await client.PostAsJsonAsync("/api/devices", new
        {
            externalId,
            name = "Basement Sensor",
            sensorType = "temperature"
        });

        await client.PostAsJsonAsync("/api/telemetry", new
        {
            deviceExternalId = externalId,
            metricType = "temperature",
            metricValue = 19.1m,
            eventTimeUtc = DateTime.UtcNow.AddMinutes(-5)
        });

        await client.PostAsJsonAsync("/api/telemetry", new
        {
            deviceExternalId = externalId,
            metricType = "temperature",
            metricValue = 21.7m,
            eventTimeUtc = DateTime.UtcNow
        });

        var listResponse = await client.GetAsync("/api/devices");

        Assert.Equal(HttpStatusCode.OK, listResponse.StatusCode);

        using var json = JsonDocument.Parse(await listResponse.Content.ReadAsStringAsync());
        var item = json.RootElement.EnumerateArray().Single(x => x.GetProperty("externalId").GetString() == externalId);

        Assert.Equal(21.7m, item.GetProperty("latestMetricValue").GetDecimal());
        Assert.Equal("temperature", item.GetProperty("latestMetricType").GetString());
    }
}
