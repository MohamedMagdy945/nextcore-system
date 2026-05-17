
using Discount.Grpc.Protos;

namespace Basket.Application.GerpcService
{
    public class DiscountGrpcSerivce
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountGrpcClient;

        public DiscountGrpcSerivce(DiscountProtoService.DiscountProtoServiceClient discountGrpcClient)
        {
            _discountGrpcClient = discountGrpcClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            var response = await _discountGrpcClient.GetDiscountAsync(request);
            return response;
        }

    }
}
