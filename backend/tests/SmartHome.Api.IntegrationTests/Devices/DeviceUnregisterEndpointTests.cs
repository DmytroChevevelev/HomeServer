using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Api.Infrastructure;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Devices;

public class DeviceUnregisterEndpointTests
{
    [Fact]
    public async Task Delete_Device_Removes_Related_Telemetry_Rows()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var (deviceId, externalId) = await RegisterDeviceAsync(client, "ext-unregister-cleanup");

        await client.PostAsJsonAsync("/api/telemetry", new
        {
            deviceExternalId = externalId,
            metricType = "temperature",
            metricValue = 18.5m,
            eventTimeUtc = DateTime.UtcNow.AddMinutes(-2)
        });

        await client.PostAsJsonAsync("/api/telemetry", new
        {
            deviceExternalId = externalId,
            metricType = "temperature",
            metricValue = 19.5m,
            eventTimeUtc = DateTime.UtcNow.AddMinutes(-1)
        });

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SmartHomeDbContext>();
            Assert.Equal(2, db.TelemetryReadings.Count(reading => reading.DeviceId == deviceId));
        }

        var deleteResponse = await client.DeleteAsync($"/api/devices/{deviceId}");
        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);

        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<SmartHomeDbContext>();
            Assert.Equal(0, db.TelemetryReadings.Count(reading => reading.DeviceId == deviceId));
        }
    }

    private static async Task<(Guid DeviceId, string ExternalId)> RegisterDeviceAsync(HttpClient client, string externalId)
    {
        var registerResponse = await client.PostAsJsonAsync("/api/devices", new
        {
            externalId,
            name = "Cleanup Sensor",
            sensorType = "temperature",
            isEnabled = true
        });

        registerResponse.EnsureSuccessStatusCode();

        var payload = await registerResponse.Content.ReadFromJsonAsync<DeviceRegisterResponse>();
        return (payload!.Id, externalId);
    }

    private sealed record DeviceRegisterResponse(Guid Id);
}
