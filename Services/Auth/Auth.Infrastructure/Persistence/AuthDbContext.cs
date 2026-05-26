using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence
{
    public class AuthDbContext
        : DbContext, IAuthDbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        DbSet<User> IAuthDbContext.Users => throw new NotImplementedException();

        DbSet<Role> IAuthDbContext.Roles => throw new NotImplementedException();

        DbSet<UserRole> IAuthDbContext.UserRoles => throw new NotImplementedException();

        DbSet<RefreshToken> IAuthDbContext.RefreshTokens => throw new NotImplementedException();

        DbSet<Permission> IAuthDbContext.Permissions => throw new NotImplementedException();

        DbSet<RolePermission> IAuthDbContext.RolePermissions => throw new NotImplementedException();

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AuthDbContext).Assembly);
        }
    }
}
