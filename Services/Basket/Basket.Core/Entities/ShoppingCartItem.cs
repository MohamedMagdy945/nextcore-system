namespace Basket.Core.Entities
{
    public class ShoppingCartItem
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string ImageFile { get; set; } = string.Empty;

    }
}
