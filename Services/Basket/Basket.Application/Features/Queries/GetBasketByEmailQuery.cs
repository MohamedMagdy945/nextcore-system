using Basket.Application.Responses;
using Basket.Core.Repositories;
using Mapster;
using MediatR;

namespace Basket.Application.Features.Queries
{
    public record GetBasketByEmailQuery(string Email)
        : IRequest<ShoppingCartResponse>;
    public class GetBasketByEmailHandler :
        IRequestHandler<GetBasketByEmailQuery, ShoppingCartResponse>
    {
        private readonly IBasketRepository _repository;
        public GetBasketByEmailHandler(IBasketRepository repository)
        {
            _repository = repository;
        }


        public async Task<ShoppingCartResponse> Handle(GetBasketByEmailQuery request, CancellationToken cancellationToken)
        {
            var shoppingCart = await _repository.GetCartAsync(request.Email);
            var shoppingCartResponse = shoppingCart.Adapt<ShoppingCartResponse>();
            return shoppingCartResponse;
        }
    }
}
