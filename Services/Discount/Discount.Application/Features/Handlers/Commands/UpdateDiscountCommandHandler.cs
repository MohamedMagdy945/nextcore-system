using Discount.Application.Features.Commands;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Mapster;
using MediatR;

namespace Discount.Application.Features.Handlers.Commands
{
    public class UpdateDiscountCommandHandler : IRequestHandler<UpdateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;

        public UpdateDiscountCommandHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<CouponModel> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = request.Adapt<Coupon>();
            await _discountRepository.UpdateDiscountAsync(coupon);
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }
    }
}
