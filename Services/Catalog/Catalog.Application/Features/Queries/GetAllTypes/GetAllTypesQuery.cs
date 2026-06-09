using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetAllTypes
{
    public record GetAllTypesQuery() : IRequest<IList<CategoryResponseDto>>;
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<CategoryResponseDto>>
    {
        private readonly ICategoryRepository _typeRepository;

        public GetAllTypesQueryHandler(
            ICategoryRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IList<CategoryResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetAllAsync();
            var typeResponses = types.Adapt<IList<CategoryResponseDto>>();
            return typeResponses;
        }
    }
}
