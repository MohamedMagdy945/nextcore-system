using Basket.Application.GerpcService;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using Mapster;
using MediatR;

namespace Basket.Application.Features.Commands.CreateShoppingCart
{
    public record CreateShoppingCartCommand(
     string email,
     List<ShoppingCartItem> Items
    ) : IRequest<ShoppingCartResponse>;

    public class CreateShoppingCartHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly DiscountGrpcSerivce _discountGrpcService;


        public CreateShoppingCartHandler(
            IBasketRepository basketRepository,
            DiscountGrpcSerivce discountGrpcSerivce)
        {
            _basketRepository = basketRepository;
            _discountGrpcService = discountGrpcSerivce;
        }


        public async Task<ShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
        {

            foreach (var item in request.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                if (coupon != null)
                {
                    item.Price -= coupon.Amount;
                }
            }

            var shoppingCart = request.Adapt<ShoppingCart>();

            shoppingCart = await _basketRepository.UpdateCartAsync(shoppingCart);
            var shoppingCartResponse = shoppingCart.Adapt<ShoppingCartResponse>();
            return shoppingCartResponse;
        }
    }
}
