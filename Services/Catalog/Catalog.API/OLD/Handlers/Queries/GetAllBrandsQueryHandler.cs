using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.OLD.Queries;
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Handlers.Queries
{
    public class GetAllBrandsQuery : IRequestHandler<OLD.Queries.GetAllBrandsQuery, IList<BrandResponseDto>>
    {
        private readonly IMapper _mapper;
        private readonly IBrandRepository _brandRepostiory;

        public GetAllBrandsQuery(
            IMapper mapper, IBrandRepository brandRepostiory)
        {
            _mapper = mapper;
            _brandRepostiory = brandRepostiory;
        }

        public async Task<IList<BrandResponseDto>> Handle(OLD.Queries.GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _brandRepostiory.GetAllAsync();
            return _mapper.Map<List<BrandResponseDto>>(brands);
        }
    }
}
