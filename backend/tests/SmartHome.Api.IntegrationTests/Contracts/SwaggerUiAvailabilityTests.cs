using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class SwaggerUiAvailabilityTests
{
    [Fact]
    public async Task SwaggerUi_Is_Available_In_Development()
    {
        using var host = new IntegrationTestHost("Development");
        using var client = host.CreateApiClient();

        var response = await client.GetAsync("/swagger/index.html");
        response.EnsureSuccessStatusCode();

        var html = await response.Content.ReadAsStringAsync();
        Assert.Contains("Swagger UI", html);
    }
}
