using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Infrastructure;

public class DatabaseMigrationDataPreservationTests
{
    [Fact]
    public async Task Database_Update_Preserves_Devices_And_TelemetryReadings_Data()
    {
        var databaseName = $"migration-preserve-{Guid.NewGuid():N}";
        InMemoryDatabaseRoot root = IntegrationTestHost.CreateSharedDatabaseRoot(databaseName);

        using (var firstHost = new IntegrationTestHost("Development", databaseName, root))
        using (var firstClient = firstHost.CreateApiClient())
        {
            var deviceResponse = await firstClient.PostAsJsonAsync("/api/devices", new
            {
                externalId = "dev-preserve-001",
                name = "Office Sensor",
                sensorType = "humidity"
            });
            Assert.Equal(HttpStatusCode.Created, deviceResponse.StatusCode);

            var telemetryResponse = await firstClient.PostAsJsonAsync("/api/telemetry", new
            {
                deviceExternalId = "dev-preserve-001",
                metricType = "humidity",
                metricValue = 49.5m,
                eventTimeUtc = DateTime.UtcNow
            });
            Assert.Equal(HttpStatusCode.Accepted, telemetryResponse.StatusCode);
        }

        using var secondHost = new IntegrationTestHost("Development", databaseName, root);
        using var secondClient = secondHost.CreateApiClient();

        var latestResponse = await secondClient.GetAsync("/api/telemetry/latest");
        latestResponse.EnsureSuccessStatusCode();

        var payload = await latestResponse.Content.ReadAsStringAsync();
        Assert.Contains("dev-preserve-001", payload);
        Assert.Contains("humidity", payload);
    }
}
