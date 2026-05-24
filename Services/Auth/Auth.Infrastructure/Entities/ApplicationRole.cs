using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        public ICollection<RolePermission> RolePermissions { get; private set; }
              = new List<RolePermission>();

        public ICollection<UserRole> UserRoles { get; private set; }
            = new List<UserRole>();
    }
}
