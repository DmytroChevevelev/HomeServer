using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Telemetry;

public class TelemetryRealtimeNotificationsTests
{
    [Fact]
    public async Task Ingest_Telemetry_Updates_Device_Latest_Projection()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var externalId = "ext-realtime-1";
        await RegisterDeviceAsync(client, externalId);

        var firstIngest = await client.PostAsJsonAsync("/api/telemetry", new
        {
            deviceExternalId = externalId,
            metricType = "temperature",
            metricValue = 19.5m,
            eventTimeUtc = DateTime.UtcNow.AddMinutes(-1)
        });

        Assert.Equal(HttpStatusCode.Accepted, firstIngest.StatusCode);

        var secondIngest = await client.PostAsJsonAsync("/api/telemetry", new
        {
            deviceExternalId = externalId,
            metricType = "temperature",
            metricValue = 21.75m,
            eventTimeUtc = DateTime.UtcNow
        });

        Assert.Equal(HttpStatusCode.Accepted, secondIngest.StatusCode);

        var devicesResponse = await client.GetAsync("/api/devices");
        devicesResponse.EnsureSuccessStatusCode();

        using var devicesDocument = JsonDocument.Parse(await devicesResponse.Content.ReadAsStringAsync());
        var match = devicesDocument.RootElement
            .EnumerateArray()
            .FirstOrDefault(item => item.GetProperty("externalId").GetString() == externalId);

        Assert.Equal(21.75m, match.GetProperty("latestMetricValue").GetDecimal());
    }

    private static async Task RegisterDeviceAsync(HttpClient client, string externalId)
    {
        var registerPayload = new
        {
            externalId,
            name = "Realtime Sensor",
            sensorType = "temperature",
            isEnabled = true
        };

        var response = await client.PostAsJsonAsync("/api/devices", registerPayload);
        response.EnsureSuccessStatusCode();
    }
}
