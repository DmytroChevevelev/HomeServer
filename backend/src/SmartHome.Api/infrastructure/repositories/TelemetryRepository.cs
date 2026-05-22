using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Models;

namespace SmartHome.Api.Infrastructure.Repositories;

public sealed class TelemetryRepository(SmartHomeDbContext db)
{
    public async Task AddAsync(TelemetryReading reading, CancellationToken ct = default)
    {
        db.TelemetryReadings.Add(reading);
        await db.SaveChangesAsync(ct);
    }

    public IQueryable<TelemetryReading> Query() => db.TelemetryReadings.AsNoTracking();
}
