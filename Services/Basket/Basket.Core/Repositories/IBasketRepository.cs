using Basket.Core.Entities;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetCartAsync(string userName);

        Task<ShoppingCart> UpdateCartAsync(ShoppingCart cart);

        Task DeleteCartAsync(string userName);
    }
}
