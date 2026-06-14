using Microsoft.EntityFrameworkCore;
using SmartHome.Api.Models;

namespace SmartHome.Api.Infrastructure;

public class SmartHomeDbContext(DbContextOptions<SmartHomeDbContext> options) : DbContext(options)
{
    public DbSet<Device> Devices => Set<Device>();
    public DbSet<TelemetryReading> TelemetryReadings => Set<TelemetryReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.HasIndex(x => x.ExternalId).IsUnique();
            entity.Property(x => x.ExternalId).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.SensorType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.RegisteredAtUtc).IsRequired();
        });

        modelBuilder.Entity<TelemetryReading>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.MetricType).HasMaxLength(50).IsRequired();
            entity.Property(x => x.MetricValue).HasColumnType("decimal(18,4)");
            entity.Property(x => x.EventTimeUtc).IsRequired();
            entity.Property(x => x.IngestedAtUtc).IsRequired();
            entity.HasOne(x => x.Device)
                .WithMany(x => x.Readings)
                .HasForeignKey(x => x.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
