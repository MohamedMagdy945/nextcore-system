namespace Auth.Infrastructure.Entities
{
    public class Permission : BaseIdentityEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<RolePermission> RolePermissions { get; private set; }
                = new List<RolePermission>();
    }
}
