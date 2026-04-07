using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetProductsByBrandQuery : IRequest<IList<ProductResponseDto>>
    {
        public string Brand { get; set; }
        public GetProductsByBrandQuery(string brand)
        {
            Brand = brand;
        }
    }
}
