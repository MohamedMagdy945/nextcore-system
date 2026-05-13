using Basket.Application.Responses;
using Basket.Core.Repositories;
using Mapster;
using MediatR;

namespace Basket.Application.Features.Queries
{
    public record GetBasketByUserNameQuery(string UserName)
        : IRequest<ShoppingCartResponse>;
    public class GetBasketByNameHandler :
        IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _repository;
        public GetBasketByNameHandler(IBasketRepository repository)
        {
            _repository = repository;
        }


        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _repository.GetCartAsync(request.UserName);
            var shoppingCartResponse = shoppingCart.Adapt<ShoppingCartResponse>();
            return shoppingCartResponse;
        }
    }
}
