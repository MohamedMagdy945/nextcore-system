namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public string Email { get; set; } = string.Empty;
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
    }
}
