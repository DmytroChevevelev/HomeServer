using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore.Storage;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Infrastructure;

public class DatabaseMigrationTests
{
    [Fact]
    public async Task Database_Can_Be_Initialized_From_Empty_State_Using_Operator_Flow()
    {
        var databaseName = $"migration-init-{Guid.NewGuid():N}";
        InMemoryDatabaseRoot root = IntegrationTestHost.CreateSharedDatabaseRoot(databaseName);

        using var host = new IntegrationTestHost("Development", databaseName, root);
        using var client = host.CreateApiClient();

        var response = await client.PostAsJsonAsync("/api/devices", new
        {
            externalId = "dev-init-001",
            name = "Kitchen Sensor",
            sensorType = "temperature"
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
