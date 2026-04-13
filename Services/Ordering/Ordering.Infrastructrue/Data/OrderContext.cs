using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;

namespace Ordering.Infrastructrue.Data
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public Task<int> SaveEntitiesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entity in ChangeTracker.Entries<EntityBase>())
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreatedDate = DateTime.UtcNow;
                        entity.Entity.CreatedBy = "Abanoub";
                        break;
                    case EntityState.Modified:
                        entity.Entity.LastModifiedBy = "Abanoub";
                        entity.Entity.LastModifiedDate = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        internal void AddAsync(object getOrders)
        {
            throw new NotImplementedException();
        }
    }
}
