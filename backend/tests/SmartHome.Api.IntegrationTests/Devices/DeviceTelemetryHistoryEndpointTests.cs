using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Devices;

public class DeviceTelemetryHistoryEndpointTests
{
    [Fact]
    public async Task Get_Device_Telemetry_Defaults_To_Latest_100_Newest_First()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var (deviceId, externalId) = await RegisterDeviceAsync(client, "ext-history-default");
        await IngestSequenceAsync(client, externalId, 120);

        var response = await client.GetAsync($"/api/devices/{deviceId}/telemetry");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rows = document.RootElement.EnumerateArray().ToArray();

        Assert.Equal(100, rows.Length);
        Assert.Equal(119m, rows[0].GetProperty("metricValue").GetDecimal());
        Assert.Equal(20m, rows[^1].GetProperty("metricValue").GetDecimal());
    }

    [Fact]
    public async Task Get_Device_Telemetry_Respects_Custom_Limit()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var (deviceId, externalId) = await RegisterDeviceAsync(client, "ext-history-limit");
        await IngestSequenceAsync(client, externalId, 10);

        var response = await client.GetAsync($"/api/devices/{deviceId}/telemetry?limit=3");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var rows = document.RootElement.EnumerateArray().ToArray();

        Assert.Equal(3, rows.Length);
        Assert.Equal(9m, rows[0].GetProperty("metricValue").GetDecimal());
        Assert.Equal(7m, rows[2].GetProperty("metricValue").GetDecimal());
    }

    [Fact]
    public async Task Get_Device_Telemetry_Returns_BadRequest_For_Invalid_Limit()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var (deviceId, _) = await RegisterDeviceAsync(client, "ext-history-bad-limit");

        var response = await client.GetAsync($"/api/devices/{deviceId}/telemetry?limit=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_Device_Telemetry_Returns_NotFound_For_Unknown_Device()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var response = await client.GetAsync($"/api/devices/{Guid.NewGuid()}/telemetry?limit=10");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static async Task<(Guid DeviceId, string ExternalId)> RegisterDeviceAsync(HttpClient client, string externalId)
    {
        var registerResponse = await client.PostAsJsonAsync("/api/devices", new
        {
            externalId,
            name = "History Sensor",
            sensorType = "temperature",
            isEnabled = true
        });

        registerResponse.EnsureSuccessStatusCode();

        using var document = JsonDocument.Parse(await registerResponse.Content.ReadAsStringAsync());
        var deviceId = document.RootElement.GetProperty("id").GetGuid();

        return (deviceId, externalId);
    }

    private static async Task IngestSequenceAsync(HttpClient client, string externalId, int count)
    {
        var start = DateTime.UtcNow.AddMinutes(-count);

        for (var i = 0; i < count; i++)
        {
            var response = await client.PostAsJsonAsync("/api/telemetry", new
            {
                deviceExternalId = externalId,
                metricType = "temperature",
                metricValue = (decimal)i,
                eventTimeUtc = start.AddMinutes(i)
            });

            response.EnsureSuccessStatusCode();
        }
    }
}
