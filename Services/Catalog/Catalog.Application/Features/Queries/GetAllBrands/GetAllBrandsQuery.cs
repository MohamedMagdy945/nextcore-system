using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetAllBrands
{
    public record GetAllBrandsQuery() : IRequest<IList<BrandResponseDto>>;
    public class GetAllBrandsHandler : IRequestHandler<GetAllBrandsQuery, IList<BrandResponseDto>>
    {
        private readonly IBrandRepository _brandRepository;

        public GetAllBrandsHandler(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IList<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllAsync();

            return brands.Adapt<IList<BrandResponseDto>>();
        }
    }
}
