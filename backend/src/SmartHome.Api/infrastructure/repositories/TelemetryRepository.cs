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

    public async Task<TelemetryReading?> GetLatestForDeviceAsync(Guid deviceId, CancellationToken ct = default)
        => await db.TelemetryReadings
            .AsNoTracking()
            .Where(reading => reading.DeviceId == deviceId)
            .OrderByDescending(reading => reading.EventTimeUtc)
            .FirstOrDefaultAsync(ct);

    public async Task<List<TelemetryReading>> ListForDeviceAsync(
        Guid deviceId,
        DateTime? fromUtc,
        DateTime? toUtc,
        int limit,
        CancellationToken ct = default)
    {
        var safeLimit = Math.Clamp(limit, 1, 500);

        var query = db.TelemetryReadings
            .AsNoTracking()
            .Where(reading => reading.DeviceId == deviceId);

        if (fromUtc.HasValue)
        {
            query = query.Where(reading => reading.EventTimeUtc >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            query = query.Where(reading => reading.EventTimeUtc <= toUtc.Value);
        }

        return await query
            .OrderByDescending(reading => reading.EventTimeUtc)
            .Take(safeLimit)
            .ToListAsync(ct);
    }
}
