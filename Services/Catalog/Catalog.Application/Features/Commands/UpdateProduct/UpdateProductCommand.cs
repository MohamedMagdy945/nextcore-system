using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Commands.UpdateProduct
{
    public record UpdateProductCommand(
        string Id, string Name, string Description,
        string Summary, decimal Price,
        ProductBrand Brand, ProductType Type, string ImageFile
    ) : IRequest<bool>;
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        public UpdateProductHandler(IProductRepository
            productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = request.Adapt<Product>();
            var isUpdated = await _productRepository.UpdateAsync(productEntity);
            return isUpdated;
        }
    }
}
