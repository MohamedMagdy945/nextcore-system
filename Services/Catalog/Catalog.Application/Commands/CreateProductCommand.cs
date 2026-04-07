using Catalog.Application.Responses;
using Catalog.Core.Entities;
using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Application.Commands
{
    public class CreateProductCommand : IRequest<ProductResponseDto>
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
