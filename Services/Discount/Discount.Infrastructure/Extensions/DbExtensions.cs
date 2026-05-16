using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Discount.Infrastructure.Extensions
{
    public static class DbExtensions
    {
        public static async Task<IHost> MigrateDatabaseAsync<TContext>(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var config = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Discount DB migration started.");

                await ApplyMigrationAsync(config);

                logger.LogInformation("Discount DB migration completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating database.");
                throw;
            }

            return host;
        }

        private static async Task ApplyMigrationAsync(IConfiguration config)
        {
            var connectionString =
                config.GetValue<string>("DatabaseSettings:ConnectionString");

            await using var connection =
                new NpgsqlConnection(connectionString);

            await connection.OpenAsync();

            // Create table
            var createTableSql = @"
                CREATE TABLE IF NOT EXISTS Coupon
                (
                    Id SERIAL PRIMARY KEY,
                    ProductName VARCHAR(500) NOT NULL UNIQUE,
                    Description TEXT,
                    Amount INT
                );
            ";

            await using (var createCmd = new NpgsqlCommand(createTableSql, connection))
            {
                await createCmd.ExecuteNonQueryAsync();
            }

            // Seed data (no C# check, DB handles duplication)
            var seedSql = @"
                INSERT INTO Coupon (ProductName, Description, Amount)
                VALUES
                ('Adidas Shoes', 'Discount', 600),
                ('PowerFit Cricket Shoes', 'Discount', 700)
                ON CONFLICT (ProductName) DO NOTHING;
            ";

            await using (var seedCmd = new NpgsqlCommand(seedSql, connection))
            {
                await seedCmd.ExecuteNonQueryAsync();
            }
        }
    }
}