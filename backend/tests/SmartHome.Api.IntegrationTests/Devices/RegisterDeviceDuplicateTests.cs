using System.Net;
using System.Net.Http.Json;
using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Devices;

public class RegisterDeviceDuplicateTests
{
    [Fact]
    public async Task Duplicate_ExternalId_Is_Rejected_With_Conflict()
    {
        using var host = new IntegrationTestHost();
        using var client = host.CreateApiClient();

        var payload = new
        {
            externalId = "ext-duplicate-1",
            name = "Hall Sensor",
            sensorType = "humidity"
        };

        var first = await client.PostAsJsonAsync("/api/devices", payload);
        var second = await client.PostAsJsonAsync("/api/devices", payload);

        Assert.Equal(HttpStatusCode.Created, first.StatusCode);
        Assert.Equal(HttpStatusCode.Conflict, second.StatusCode);
    }
}
