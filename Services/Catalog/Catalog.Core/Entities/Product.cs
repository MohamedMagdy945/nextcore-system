using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        public ProductBrand Brand { get; set; } = new();

        public ProductType Type { get; set; } = new();

        public string ImageFile { get; set; } = string.Empty;
    }
}