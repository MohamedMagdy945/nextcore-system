using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetProductById
{
    public record GetProductByIdQuery(string Id) : IRequest<ProductResponseDto>;
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;

        public GetProductByIdQueryHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            var productResponseDto = product.Adapt<ProductResponseDto>();
            return productResponseDto;
        }
    }
}
