namespace Basket.Core.Entities
{
    public class ShoppingCart
    {
        public ShoppingCart() { }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();


    }
}
