using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.GerpcService;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Handlers.Commands
{
    public class CreateShoppingCartCommandHandler : IRequestHandler<CreateShoppingCartCommand, ShoppingCartResponse>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        private readonly DiscountGrpcSerivce _discountGrpcService;


        public CreateShoppingCartCommandHandler(
            IBasketRepository basketRepository,
            IMapper mapper,
            DiscountGrpcSerivce discountGrpcSerivce)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
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
            var shoppingCart = new ShoppingCart()
            {
                UserName = request.UserName,
                Items = request.Items
            };

            shoppingCart = await _basketRepository.UpdateBasketAsync(shoppingCart);
            var shoppingCartResponse = _mapper.Map<ShoppingCartResponse>(shoppingCart);
            return shoppingCartResponse;
        }
    }
}
