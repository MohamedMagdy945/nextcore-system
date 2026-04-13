using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructrue.Data
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=AppCoreSystemDb;Trusted_Connection=True;TrustServerCertificate=True;");

            return new OrderContext(optionsBuilder.Options);
        }
    }
}
