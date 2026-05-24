namespace Auth.Infrastructure.Entities
{
    public class UserRole
    {
        public int UserId { get; set; }

        public ApplicationUser User { get; set; } = default!;

        public Guid RoleId { get; set; }

        public ApplicationRole Role { get; set; } = default!;
    }
}
