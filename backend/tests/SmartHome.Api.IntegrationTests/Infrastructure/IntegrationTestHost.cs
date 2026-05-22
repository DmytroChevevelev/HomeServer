using System.Collections.Concurrent;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartHome.Api.Infrastructure;

namespace SmartHome.Api.IntegrationTests.Infrastructure;

public sealed class IntegrationTestHost : WebApplicationFactory<Program>
{
    private static readonly ConcurrentDictionary<string, InMemoryDatabaseRoot> SharedDatabaseRoots = new();

    private readonly string _environmentName;
    private readonly string _databaseName;
    private readonly InMemoryDatabaseRoot _databaseRoot;

    public static string Name => "WebApplicationFactory-backed integration host";

    public IntegrationTestHost(
        string environmentName = "Development",
        string? databaseName = null,
        InMemoryDatabaseRoot? databaseRoot = null)
    {
        _environmentName = environmentName;
        _databaseName = string.IsNullOrWhiteSpace(databaseName)
            ? $"smarthome-integration-{Guid.NewGuid():N}"
            : databaseName;
        _databaseRoot = databaseRoot ?? new InMemoryDatabaseRoot();
    }

    public static InMemoryDatabaseRoot CreateSharedDatabaseRoot(string databaseName)
    {
        return SharedDatabaseRoots.GetOrAdd(databaseName, _ => new InMemoryDatabaseRoot());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(_environmentName);
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<SmartHomeDbContext>>();
            services.RemoveAll<IDbContextOptionsConfiguration<SmartHomeDbContext>>();
            services.AddDbContext<SmartHomeDbContext>(options =>
                options
                    .UseInMemoryDatabase(_databaseName, _databaseRoot)
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning)));
        });
    }

    public HttpClient CreateApiClient()
    {
        return CreateClient();
    }
}
