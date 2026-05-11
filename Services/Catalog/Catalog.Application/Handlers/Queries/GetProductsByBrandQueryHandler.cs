using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public record GetProductsByBrandQuery(string Brand) : IRequest<IList<ProductResponseDto>>;
    public class GetProductsByBrandQueryHandler : IRequestHandler<GetProductsByBrandQuery, IList<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductsByBrandQueryHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<IList<ProductResponseDto>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllByBrandNameAsync(request.Brand);
            var productsResponse = products.Adapt<IList<ProductResponseDto>>();
            return productsResponse;
        }
    }
}
