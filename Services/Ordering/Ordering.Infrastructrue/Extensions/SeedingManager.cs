using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Persistence.Seeder;

namespace Ordering.Infrastructure.Extensions
{
    public static class SeedingManager
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderSeeder>>();

            await OrderSeeder.SeedAsync(context, logger);
        }
    }
}
