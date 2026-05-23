using Auth.Application.Interfaces;
using Auth.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Infrastructure.Persistence
{
    public class AuthDbContext : IdentityDbContext
        <ApplicationUser, ApplicationRole, int>, IAuthDbContext
    {
        public AuthDbContext(
        DbContextOptions<AuthDbContext> options)
        : base(options)
        {
        }
    }
}
