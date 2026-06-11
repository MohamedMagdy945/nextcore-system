using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Catalog.Core.Models;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetAllProducts
{
    public record GetAllProductsQuery(ProductParams ProductParams) : IRequest<Pagination<ProductResponseDto>>;
    public class GetAllProductsQueryHandle : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsQueryHandle(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public async Task<Pagination<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(request.ProductParams);

            var productsResponse = products.Adapt<Pagination<ProductResponseDto>>();

            return productsResponse;
        }
    }
}
