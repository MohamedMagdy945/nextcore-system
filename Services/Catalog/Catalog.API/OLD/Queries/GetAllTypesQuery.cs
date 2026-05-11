using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.OLD.Queries
{
    public class GetAllTypesQuery : IRequest<IList<TypeResponseDto>>
    {

    }
}
