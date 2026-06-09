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


        [BsonRepresentation(BsonType.ObjectId)]
        public string BrandId { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; } = string.Empty;

        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}