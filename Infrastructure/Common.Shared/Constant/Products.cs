namespace Common.Shared.Constant
{
    public static class Products
    {
        public const string View = "Products.View";
        public const string Rate = "Products.Rate";

        public static List<string> GetAll()
        {
            return new List<string>
            {
                View,
                Rate
            };
        }
    }
}
