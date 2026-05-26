using Auth.Infrastructure.Persistence.Seeder;
using Microsoft.Extensions.Logging;

namespace Auth.Infrastructure.Persistence.DatabaseSeeder
{
    public class DatabaseSeeder
    {
        private readonly RoleSeeder _roleSeeder;
        private readonly PermissionSeeder _permissionSeeder;
        private readonly UserSeeder _userSeeder;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(
            ILogger<DatabaseSeeder> logger,
            RoleSeeder roleSeeder,
            PermissionSeeder permissionSeeder,
            UserSeeder userSeeder)
        {
            _logger = logger;
            _roleSeeder = roleSeeder;
            _permissionSeeder = permissionSeeder;
            _userSeeder = userSeeder;
        }

        public async Task SeedAsync()
        {
            _logger.LogInformation("Starting database seeding...");
            await _roleSeeder.SeedAsync();
            await _permissionSeeder.SeedAsync();
            await _userSeeder.SeedAsync();
            _logger.LogInformation("Database seeding completed.");
        }
    }
}
