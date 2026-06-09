using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Responses;
using Mapster;
using MediatR;

namespace Catalog.Application.Features.Queries.GetAllTypes
{
    public record GetAllCategoriesQuery() : IRequest<IList<CategoryResponseDto>>;
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IList<CategoryResponseDto>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetAllTypesQueryHandler(
            ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IList<CategoryResponseDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            var categoryResponses = categories.Adapt<IList<CategoryResponseDto>>();
            return categoryResponses;
        }
    }
}
