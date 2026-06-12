using Basket.Core.Entities;

namespace Basket.Core.Repositories
{
    public interface IBasketRepository
    {
        Task<ShoppingCart?> GetCartAsync(string Email);

        Task<ShoppingCart> UpdateCartAsync(ShoppingCart cart);

        Task DeleteCartAsync(string Email);
    }
}
