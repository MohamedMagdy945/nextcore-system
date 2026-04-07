using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries
{
    public class GetAllProductsByNameQuery : IRequest<IList<ProductResponseDto>>
    {
        public string Name { get; set; }
        public GetAllProductsByNameQuery(string name)
        {
            Name = name;
        }
    }
}
