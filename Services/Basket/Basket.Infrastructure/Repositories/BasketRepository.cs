using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart?> GetCartAsync(string email)
        {
            var cart = await _redisCache.GetStringAsync(email);

            return string.IsNullOrEmpty(cart)
                ? null
                : JsonConvert.DeserializeObject<ShoppingCart>(cart);
        }

        public async Task<ShoppingCart> UpdateCartAsync(ShoppingCart cart)
        {
            var serializedCart = JsonConvert.SerializeObject(cart);

            await _redisCache.SetStringAsync(
                cart.Email,
                serializedCart
            );

            return cart;
        }

        public async Task DeleteCartAsync(string email)
        {
            await _redisCache.RemoveAsync(email);
        }
    }
}