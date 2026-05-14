using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Discount.Application.Features.Queries.GetDiscount
{
    public record GetDiscountQuery(string ProductName) : IRequest<CouponModel>;
    public class GetDiscountHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly IDiscountRepository _discountRepository;
        private readonly ILogger<GetDiscountHandler> _logger;

        public GetDiscountHandler(
            IDiscountRepository discountRepository,
            ILogger<GetDiscountHandler> logger
            )
        {
            _discountRepository = discountRepository;
            _logger = logger;
        }

        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _discountRepository.GetDiscountAsync(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(
                    new Status(StatusCode.NotFound, $"Discount for product name ={request.ProductName} not found"));
            }
            var couponModel = new CouponModel()
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
            };
            _logger.LogInformation($"Coupon for the request {request.ProductName} is fetched ");

            return couponModel;
        }
    }
}
