using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.OLD.Queries
{
    public class GetAllBrandsQuery : IRequest<IList<BrandResponseDto>>
    {

    }
}
