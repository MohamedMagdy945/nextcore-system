using Catalog.Application.Interfaces.Repositories;
using MediatR;

namespace Catalog.Application.Features.Commands.DeleteProduct
{
    public record DeleteProductCommand(string Id) : IRequest<bool>;
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            return await _productRepository.DeleteAsync(request.Id);
        }
    }
}
