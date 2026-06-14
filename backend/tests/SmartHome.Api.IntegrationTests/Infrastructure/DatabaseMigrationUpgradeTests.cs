using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Infrastructure;

public class DatabaseMigrationUpgradeTests
{
    [Fact]
    public async Task Database_Update_Path_Is_Idempotent_For_Existing_State()
    {
        var databaseName = $"migration-upgrade-{Guid.NewGuid():N}";
        InMemoryDatabaseRoot root = IntegrationTestHost.CreateSharedDatabaseRoot(databaseName);

        using (var firstHost = new IntegrationTestHost("Development", databaseName, root))
        using (var firstClient = firstHost.CreateApiClient())
        {
            var createResponse = await firstClient.PostAsJsonAsync("/api/devices", new
            {
                externalId = "dev-upgrade-001",
                name = "Boiler Sensor",
                sensorType = "temperature"
            });
            Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        }

        using var secondHost = new IntegrationTestHost("Development", databaseName, root);
        using var secondClient = secondHost.CreateApiClient();

        var listResponse = await secondClient.GetAsync("/api/devices");
        listResponse.EnsureSuccessStatusCode();

        var payload = await listResponse.Content.ReadAsStringAsync();
        Assert.Contains("dev-upgrade-001", payload);
    }
}
