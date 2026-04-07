using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Application.Handlers.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        public UpdateProductCommandHandler(
            IMapper mapper, IProductRepository
            productRepository)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var productEntity = new Product()
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                ImageFile = request.ImageFile,
                Price = request.Price,
                Summary = request.Summary,
                Brand = request.Brand,
                Type = request.Type,
            };
            var product = await _productRepository.UpdateAsync(productEntity);
            return product;
        }
    }
}
