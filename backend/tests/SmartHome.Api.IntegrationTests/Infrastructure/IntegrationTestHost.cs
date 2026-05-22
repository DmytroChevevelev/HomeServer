using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartHome.Api.Infrastructure;

namespace SmartHome.Api.IntegrationTests.Infrastructure;

public sealed class IntegrationTestHost(string environmentName = "Development") : WebApplicationFactory<Program>
{
    public static string Name => "WebApplicationFactory-backed integration host";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(environmentName);
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<SmartHomeDbContext>>();
            services.AddDbContext<SmartHomeDbContext>(options =>
                options.UseInMemoryDatabase($"smarthome-integration-{Guid.NewGuid():N}"));
        });
    }

    public HttpClient CreateApiClient()
    {
        return CreateClient();
    }
}
