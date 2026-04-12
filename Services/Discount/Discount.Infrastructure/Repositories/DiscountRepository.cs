using Dapper;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Microsoft.Extensions.Configuration;

namespace Discount.Infrastructure.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetValue<string>("DataBaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

            if (coupon == null)
                return new Coupon
                { ProductName = "No Discount", Amount = 0, Description = "No Discount Available for this product" };

            return coupon;
        }
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetValue<string>("DataBaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("INSERT INTO Coupon " +
                "(ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new
                {
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                }
            );

            if (affected == 0)
                return false;

            return true;
        }
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {

            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetValue<string>("DataBaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("UPDATE Coupon SET " +
                "(ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)" +
                "WHERE Id = @Id",
                new
                {
                    Id = coupon.Id,
                    ProductName = coupon.ProductName,
                    Description = coupon.Description,
                    Amount = coupon.Amount
                }
            );

            if (affected == 0)
                return false;

            return true;
        }
        public async Task<bool> DeleteDiscount(string productName)
        {
            await using var connection = new Npgsql.NpgsqlConnection(
                _configuration.GetValue<string>("DataBaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync("DELETE FROM Coupon" +
                "WHERE ProductName = @ProductName",
                new
                {
                    ProductName = productName,
                }
            );

            if (affected == 0)
                return false;

            return true;
        }
    }
}
