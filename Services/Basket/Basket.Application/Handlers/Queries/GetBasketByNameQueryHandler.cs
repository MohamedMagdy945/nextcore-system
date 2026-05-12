using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repositories;
using Mapster;
using MediatR;

namespace Basket.Application.Handlers.Queries
{
    public class GetBasketByNameQueryHandler : IRequestHandler<GetBasketByUserNameQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _repository;
        public GetBasketByNameQueryHandler(IBasketRepository repository)
        {
            _repository = repository;
        }


        public async Task<ShoppingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _repository.GetBasketAsync(request.UserName);
            var shoppingCartResponse = shoppingCart.Adapt<ShoppingCartResponse>();
            return shoppingCartResponse;
        }
    }
}
