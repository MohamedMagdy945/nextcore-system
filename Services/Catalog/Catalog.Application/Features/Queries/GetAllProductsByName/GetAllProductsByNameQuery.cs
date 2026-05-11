using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetAllProductsByName
{
    public record GetAllProductsByNameQuery(string Name) : IRequest<IList<ProductResponseDto>>;
    public class GetAllProductsByNameHandler : IRequestHandler<GetAllProductsByNameQuery, IList<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;

        public GetAllProductsByNameHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IList<ProductResponseDto>> Handle(GetAllProductsByNameQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllByNameAsync(request.Name);
            var productsResponse = products.Adapt<IList<ProductResponseDto>>();
            return productsResponse;
        }
    }
}
