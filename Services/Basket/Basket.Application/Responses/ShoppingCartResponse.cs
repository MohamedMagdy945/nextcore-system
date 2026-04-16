using Basket.Core.Entities;

namespace Basket.Application.Responses
{
    public class ShoppingCartResponse
    {
        public string UserName { get; set; } = string.Empty;
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCartResponse(string userName)
        {
            UserName = userName;
        }
        public double TotalPrice
        {
            get
            {
                double total = 0;
                foreach (var item in Items)
                {
                    total += item.Price * item.Quantity;
                }
                return total;
            }
        }
    }
}
