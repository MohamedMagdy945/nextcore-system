using Catalog.Core.Entities;

namespace Catalog.Application.Responses
{
    public class ProductResponseDto
    {

        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public int RatingsAverage { get; set; }
        public int RatingsQuantity { get; set; }
        public decimal Price { get; set; }

        public Brand Brand { get; set; } = default!;
        public Category Category { get; set; } = default!;

        public List<string> ImageUrls { get; set; } = new();
    }
}
