using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces
{
    public interface IAuthDbContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Role> Roles { get; }
        public DbSet<UserRole> UserRoles { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }
        public DbSet<Permission> Permissions { get; }
        public DbSet<RolePermission> RolePermissions { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
