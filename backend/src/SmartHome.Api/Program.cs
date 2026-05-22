using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SmartHome.Api.Api.Endpoints;
using SmartHome.Api.Api.Errors;
using SmartHome.Api.Api.Middleware;
using SmartHome.Api.Infrastructure;
using SmartHome.Api.Infrastructure.Repositories;
using SmartHome.Api.Services;

var builder = WebApplication.CreateBuilder(args);

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
});

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

var docsEnabled = app.Environment.IsDevelopment()
    ? builder.Configuration.GetValue("Docs:EnabledInDevelopment", true)
    : builder.Configuration.GetValue("Docs:EnabledInNonDevelopment", false);

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
