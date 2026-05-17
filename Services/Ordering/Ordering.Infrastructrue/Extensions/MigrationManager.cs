using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Extensions
{
    public static class MigrationManager
    {
        public static void ApplyMigrations(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var logger = scope.ServiceProvider
                .GetRequiredService<ILogger<OrderDbContext>>();

            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();

                logger.LogInformation("Applying migrations...");

                dbContext.Database.Migrate();

                logger.LogInformation("Migrations applied successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying migrations.");
                throw;
            }
        }
    }
}
