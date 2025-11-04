using Microsoft.EntityFrameworkCore;

namespace HomeWebApi.Data
{
    public class HomeSensorsContext : DbContext
    {
        public HomeSensorsContext(DbContextOptions<HomeSensorsContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for your entities here
        public DbSet<Sensor> Sensors { get; set; }
    }
}