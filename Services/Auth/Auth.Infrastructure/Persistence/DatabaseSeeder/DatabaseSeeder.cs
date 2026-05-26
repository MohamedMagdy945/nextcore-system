using Auth.Infrastructure.Persistence.Seeder;

namespace Auth.Infrastructure.Persistence.DatabaseSeeder
{
    public class DatabaseSeeder
    {
        private readonly RoleSeeder _roleSeeder;
        private readonly PermissionSeeder _permissionSeeder;
        private readonly UserSeeder _userSeeder;

        public DatabaseSeeder(
            RoleSeeder roleSeeder,
            PermissionSeeder permissionSeeder,
            UserSeeder userSeeder)
        {
            _roleSeeder = roleSeeder;
            _permissionSeeder = permissionSeeder;
            _userSeeder = userSeeder;
        }

        public async Task SeedAsync()
        {
            await _roleSeeder.SeedAsync();
            await _permissionSeeder.SeedAsync();
            await _userSeeder.SeedAsync();
        }
    }
}
