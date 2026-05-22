using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Models;

namespace SmartHome.Api.Infrastructure.Repositories;

public sealed class DeviceRepository(SmartHomeDbContext db)
{
    public async Task<bool> ExistsByExternalIdAsync(string externalId, CancellationToken ct = default)
        => await db.Devices.AnyAsync(d => d.ExternalId == externalId, ct);

    public async Task<Device?> GetByExternalIdAsync(string externalId, CancellationToken ct = default)
        => await db.Devices.FirstOrDefaultAsync(d => d.ExternalId == externalId, ct);

    public async Task AddAsync(Device device, CancellationToken ct = default)
    {
        db.Devices.Add(device);
        await db.SaveChangesAsync(ct);
    }

    public IQueryable<Device> Query() => db.Devices.AsQueryable();
}
