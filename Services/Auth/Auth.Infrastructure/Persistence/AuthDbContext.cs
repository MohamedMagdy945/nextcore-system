using Auth.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence
{
    public class AuthDbContext
        : DbContext, IAuthDbContext
    {
        public AuthDbContext(
        DbContextOptions<AuthDbContext> options)
        : base(options)
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
