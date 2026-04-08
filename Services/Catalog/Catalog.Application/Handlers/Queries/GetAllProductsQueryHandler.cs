using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllProductsQueryHandle : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponseDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandle(
            IProductRepository productRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }


        public async Task<Pagination<ProductResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productRepository.GetAllAsync(request.CatalogSpecParams);
            var productsResponse = _mapper.Map<Pagination<ProductResponseDto>>(products);
            return productsResponse;
        }
    }
}
