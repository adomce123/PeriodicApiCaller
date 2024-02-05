using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PeriodicApiCaller.Persistence;

namespace PeriodicApiCaller;

public static class DatabaseInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<WeatherInfoDbContext>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                logger.LogInformation("Applying database migrations...");
                dbContext.Database.Migrate();
                logger.LogInformation("Database migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                throw;
            }
        }
    }
}
