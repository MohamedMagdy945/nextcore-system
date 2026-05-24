namespace Auth.Infrastructure.Entities
{
    internal class BaseIdentityEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
    }
}
