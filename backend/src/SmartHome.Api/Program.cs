using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using SmartHome.Api.Api.Endpoints;
using SmartHome.Api.Api.Errors;
using SmartHome.Api.Api.Middleware;
using SmartHome.Api.Infrastructure;
using SmartHome.Api.Infrastructure.Repositories;
using SmartHome.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());

builder.Services.AddDbContext<SmartHomeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<DeviceRepository>();
builder.Services.AddScoped<TelemetryRepository>();
builder.Services.AddScoped<TelemetryIngestionService>();
builder.Services.AddScoped<LatestTelemetryQueryService>();
builder.Services.AddSingleton<DeviceStatusEvaluator>();
builder.Services.AddSingleton<ValidationErrorFactory>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Smart Home Portal API",
        Version = "1.0.0",
        Description = "Interactive API documentation for Smart Home backend endpoints."
    });

    var xmlFile = $"{typeof(Program).Assembly.GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var allowedOrigins = (builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
    .Select(origin => origin.Trim().TrimEnd('/'))
    .Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        // Keep explicit allowlist behavior to avoid accidental wildcard exposure.
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSerilogRequestLogging();

if (allowedOrigins.Length == 0)
{
    app.Logger.LogWarning("Cors:AllowedOrigins is empty; browser clients will be blocked by CORS policy until origins are configured.");
}

var migrationLogger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseMigration");
var logMigrationStateOnStartup = builder.Configuration.GetValue("Database:LogMigrationStateOnStartup", true);
var applyMigrationsOnStartup = builder.Configuration.GetValue("Database:ApplyMigrationsOnStartup", false);

if (applyMigrationsOnStartup)
{
    migrationLogger.LogWarning("Database:ApplyMigrationsOnStartup is enabled, but automatic migration execution is disabled by policy. Use operator-run migrations (dotnet ef database update).");
}

if (logMigrationStateOnStartup)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<SmartHomeDbContext>();
    try
    {
        if (dbContext.Database.ProviderName?.Contains("InMemory", StringComparison.OrdinalIgnoreCase) == true)
        {
            migrationLogger.LogInformation("Migration-state diagnostics skipped for in-memory provider.");
        }
        else
        {
            var pendingMigrations = dbContext.Database.GetPendingMigrations().ToArray();
            if (pendingMigrations.Length == 0)
            {
                migrationLogger.LogInformation("Database schema is current. No pending migrations detected.");
            }
            else
            {
                migrationLogger.LogWarning("Database has {Count} pending migrations: {Migrations}", pendingMigrations.Length, string.Join(", ", pendingMigrations));
            }
        }
    }
    catch (Exception ex)
    {
        migrationLogger.LogWarning(ex, "Unable to evaluate migration state on startup. Run operator migration commands to verify schema state.");
    }
}

var docsEnabled = app.Environment.IsDevelopment()
    ? builder.Configuration.GetValue("Docs:EnabledInDevelopment", true)
    : builder.Configuration.GetValue("Docs:EnabledInNonDevelopment", false);
var requireOpenApiMetadataDescriptions = builder.Configuration.GetValue("Docs:RequireOpenApiMetadataDescriptions", true);

if (requireOpenApiMetadataDescriptions)
{
    app.Logger.LogInformation("OpenAPI metadata descriptions are required for modified endpoints in this feature.");
}

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments(DocumentationPaths.UiRoot))
    {
        var logger = context.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("SwaggerDocs");
        logger.LogInformation("Documentation endpoint requested: {Path}", context.Request.Path);
        await next();
        if (context.Response.StatusCode >= StatusCodes.Status400BadRequest)
        {
            logger.LogWarning("Documentation request failed with {StatusCode} for {Path}", context.Response.StatusCode, context.Request.Path);
        }

        return;
    }

    await next();
});

if (docsEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(DocumentationPaths.OpenApiJsonPath, "Smart Home Portal API v1");
        options.RoutePrefix = DocumentationPaths.UiRoutePrefix;
    });
}
else
{
    app.MapGet(DocumentationPaths.UiRoutePrefix, () => Results.NotFound(new { message = "Documentation is unavailable in this environment." }));
    app.MapGet($"{DocumentationPaths.UiRoutePrefix}/{{**path}}", () => Results.NotFound(new { message = "Documentation is unavailable in this environment." }));
}

app.UseMiddleware<RequestCorrelationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<IngestionRateLimiterMiddleware>();
app.UseCors("Frontend");

app.MapDevicesEndpoints();
app.MapTelemetryEndpoints();

app.Run();

public partial class Program { }
