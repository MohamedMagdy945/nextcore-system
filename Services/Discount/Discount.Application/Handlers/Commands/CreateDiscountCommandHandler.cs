using Discount.Application.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Mapster;
using MediatR;

namespace Discount.Application.Handlers.Commands
{
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        public CreateDiscountCommandHandler(
            IDiscountRepository discountRepository
            )
        {
            _discountRepository = discountRepository;
        }

        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = request.Adapt<Coupon>();

            await _discountRepository.CreateDiscountAsync(coupon);

            var couponModel = coupon.Adapt<CouponModel>();

            return couponModel;

        }
    }
}
