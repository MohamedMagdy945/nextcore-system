using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Persistence.Seeder
{
    public class OrderSeeder
    {
        public static async Task SeedAsync(OrderDbContext context, ILogger<OrderSeeder> logger)
        {
            if (await context.Orders.AnyAsync())
                return;

            await context.Orders.AddRangeAsync(GetPreconfiguredOrders());
            await context.SaveChangesAsync();

            logger.LogInformation("Database seeding completed for Orders DB.");
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "johndoe",
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    AddressLine = "123 Main St",
                    Country = "USA",
                    State = "NY",
                    TotalPrice = 100.5m,
                    CardName = "Visa",
                    CardNumber = "4111111111111111",
                    Expiration = "12/2025",
                    CVV = "123",
                    CreatedBy = "system"
                }
            };
        }
    }
}