using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Commands.CreateProduct
{
    public record CreateProductCommand(
        string Name,
        string Description,
        string Summary,
        decimal Price,
        ProductBrand Brand,
        ProductType Type,
        string ImageFile
    ) : IRequest<ProductResponseDto>;
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponseDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(
            IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = request.Adapt<Product>();
            var newProduct = await _productRepository.CreateAsync(productEntity);
            var productResponse = newProduct.Adapt<ProductResponseDto>();
            return productResponse;
        }
    }
}
