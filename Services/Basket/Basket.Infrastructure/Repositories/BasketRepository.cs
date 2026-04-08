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

        public async Task<ShoppingCart> GetBasketAsync(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart cart)
        {
            var basket = await _redisCache.GetStringAsync(cart.UserName);

            if (string.IsNullOrEmpty(basket))
            {
                // logic return
            }

            await _redisCache.SetStringAsync(cart.UserName, JsonConvert.SerializeObject(cart));
            return await GetBasketAsync(cart.UserName);
        }

        public async Task DeleteBasket(string userName)
        {
            var basket = _redisCache.GetStringAsync(userName);
            if (basket == null)
            {
                await _redisCache.RemoveAsync(userName);
            }

        }



    }
}
