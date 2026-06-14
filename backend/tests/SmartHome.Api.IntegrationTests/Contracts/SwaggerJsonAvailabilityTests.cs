using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class SwaggerJsonAvailabilityTests
{
    [Fact]
    public async Task SwaggerJson_Is_Available_In_Development()
    {
        using var host = new IntegrationTestHost("Development");
        using var client = host.CreateApiClient();

        var response = await client.GetAsync("/swagger/v1/swagger.json");
        response.EnsureSuccessStatusCode();

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"openapi\"", body);
    }
}
