using Discount.Application.Features.Commands.CreateDiscount;
using Discount.Application.Features.Commands.DeleteDiscount;
using Discount.Application.Features.Commands.UpdateDiscount;
using Discount.Application.Features.Queries.GetDiscount;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.API.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _mediator;

        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var result = await _mediator.Send(query);
            return result;
        }

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new CreateDiscountCommand(request.Coupon.ProductName,
                request.Coupon.Description, request.Coupon.Amount);


            var result = await _mediator.Send(cmd);
            return result;
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var cmd = new UpdateDiscountCommand(request.Coupon.Id, request.Coupon.ProductName,
                request.Coupon.Description, request.Coupon.Amount);

            var result = await _mediator.Send(cmd);
            return result;
        }

        public override async Task<DeleteDiscountResponse> DeletDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var cmd = new DeleteDiscountCommand(request.ProductName);
            var deleted = await _mediator.Send(cmd);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }
    }
}
