namespace Auth.Domain.Entities
{
    public class Permission : BaseIdentityEntity
    {
        public string Name { get; set; } = null!;
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
