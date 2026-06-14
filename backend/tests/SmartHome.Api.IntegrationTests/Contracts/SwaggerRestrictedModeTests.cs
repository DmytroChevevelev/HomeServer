using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class SwaggerRestrictedModeTests
{
    [Fact]
    public async Task SwaggerEndpoints_Are_NotAvailable_In_NonDevelopment_ByDefault()
    {
        using var host = new IntegrationTestHost("Production");
        using var client = host.CreateApiClient();

        var uiResponse = await client.GetAsync("/swagger");
        var uiIndexResponse = await client.GetAsync("/swagger/index.html");
        var jsonResponse = await client.GetAsync("/swagger/v1/swagger.json");

        Assert.Equal(System.Net.HttpStatusCode.NotFound, uiResponse.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, uiIndexResponse.StatusCode);
        Assert.Equal(System.Net.HttpStatusCode.NotFound, jsonResponse.StatusCode);
    }
}
