using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetProductByIdQuery : IRequest<ProductResponseDto>
    {
        public string Id { get; set; } = string.Empty;
        public GetProductByIdQuery(string id)
        {
            Id = id;
        }
    }
}
