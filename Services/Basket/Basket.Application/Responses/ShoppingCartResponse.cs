using Basket.Core.Entities;

namespace Basket.Application.Responses
{
    public record ShoppingCartResponse
    {
        public string UserName { get; set; } = string.Empty;
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }
        }
    }
}
