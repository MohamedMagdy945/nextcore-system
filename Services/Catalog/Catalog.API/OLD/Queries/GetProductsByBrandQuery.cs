using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.OLD.Queries
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
