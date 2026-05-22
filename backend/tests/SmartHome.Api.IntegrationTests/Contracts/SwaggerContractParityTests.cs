using SmartHome.Api.IntegrationTests.Infrastructure;
using Xunit;

namespace SmartHome.Api.IntegrationTests.Contracts;

public class SwaggerContractParityTests
{
    [Fact]
    public async Task Swagger_Docs_Access_Routes_Match_Source_Contract_Expectations()
    {
        var repoRoot = GetRepositoryRoot();
        var contractPath = Path.Combine(repoRoot, "specs", "002-swagger-endpoints", "contracts", "openapi-docs.yaml");
        var contractText = await File.ReadAllTextAsync(contractPath);

        Assert.Contains("/swagger:", contractText);
        Assert.Contains("/swagger/index.html:", contractText);
        Assert.Contains("/swagger/v1/swagger.json:", contractText);

        using var host = new IntegrationTestHost("Development");
        using var client = host.CreateApiClient();

        Assert.True((await client.GetAsync("/swagger")).IsSuccessStatusCode);
        Assert.True((await client.GetAsync("/swagger/index.html")).IsSuccessStatusCode);
        Assert.True((await client.GetAsync("/swagger/v1/swagger.json")).IsSuccessStatusCode);
    }

    private static string GetRepositoryRoot()
    {
        var current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current is not null)
        {
            if (Directory.Exists(Path.Combine(current.FullName, "specs")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new DirectoryNotFoundException("Unable to locate repository root containing specs folder.");
    }
}
