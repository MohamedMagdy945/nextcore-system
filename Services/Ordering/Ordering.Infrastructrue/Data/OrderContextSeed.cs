using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructrue.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILogger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                await context.AddRangeAsync(GetOrders());
                await context.SaveChangesAsync();
                logger.LogInformation($"Seeded Orders.{typeof(OrderContext).Name}!");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new Order(){
                    UserName="johndoe"  ,
                    FirstName="John"  ,
                    LastName="Doe"  ,
                    EmailAddress="johndoe@example.com",
                    AddressLine="123 Main St"  ,
                    TotalPrice=100.5,
                    Country="USA"  ,
                    State="NY"  ,
                    CardName= "Visa"  ,
                    CardNumber="4111111111111111"  ,
                    CreatedBy ="johndoe"  ,
                    Expiration ="12/2025"  ,
                    CVV="123"
                }

            };
        }
    }
}
