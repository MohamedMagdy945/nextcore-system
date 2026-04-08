using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQuery : IRequest<Pagination<ProductResponseDto>>
    {
        public CatalogSpecParams CatalogSpecParams { get; set; }
        public GetAllProductsQuery(CatalogSpecParams catalog)
        {

            CatalogSpecParams = catalog;
        }
    }
}
