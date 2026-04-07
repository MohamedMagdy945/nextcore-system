using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;

        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public decimal Price { get; set; }

        public ProductBrand Brand { get; set; } = default!;
        public ProductType Type { get; set; } = default!;

        public string ImageFile { get; set; } = string.Empty;
    }

}