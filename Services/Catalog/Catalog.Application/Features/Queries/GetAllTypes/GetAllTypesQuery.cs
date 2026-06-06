using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetAllTypes
{
    public record GetAllTypesQuery() : IRequest<IList<TypeResponseDto>>;
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, IList<TypeResponseDto>>
    {
        private readonly ICategoryRepository _typeRepository;

        public GetAllTypesQueryHandler(
            ICategoryRepository typeRepository)
        {
            _typeRepository = typeRepository;
        }

        public async Task<IList<TypeResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _typeRepository.GetAllAsync();
            var typeResponses = types.Adapt<IList<TypeResponseDto>>();
            return typeResponses;
        }
    }
}
