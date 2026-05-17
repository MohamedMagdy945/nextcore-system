using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=OrderDb;User Id=sa;Password=P@ssw0rd123;Trusted_Connection=True;TrustServerCertificate=True;");

            return new OrderDbContext(optionsBuilder.Options);
        }
    }
}