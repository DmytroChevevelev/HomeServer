using Microsoft.EntityFrameworkCore;
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

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseMiddleware<RequestCorrelationMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<IngestionRateLimiterMiddleware>();
app.UseCors("Frontend");

app.MapDevicesEndpoints();
app.MapTelemetryEndpoints();

app.Run();

public partial class Program { }
