using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public ICollection<UserRole> UserRoles { get; private set; }
        = new List<UserRole>();

        public ICollection<RefreshToken> RefreshTokens { get; private set; }
        = new List<RefreshToken>();
    }
}
