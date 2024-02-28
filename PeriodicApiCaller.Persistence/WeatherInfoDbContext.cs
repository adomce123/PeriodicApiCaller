using Microsoft.EntityFrameworkCore;
using PeriodicApiCaller.Persistence.Entities;

namespace PeriodicApiCaller.Persistence
{
    public class WeatherInfoDbContext : DbContext
    {
        public DbSet<WeatherInfo> WeatherInfos { get; set; }

        public WeatherInfoDbContext(DbContextOptions<WeatherInfoDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeatherInfo>().ToTable("WeatherInfo");

            modelBuilder.Entity<WeatherInfo>()
                .Property(w => w.TemperatureC)
                .HasColumnType("decimal(7, 2)");
        }
    }
}
