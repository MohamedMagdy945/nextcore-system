using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Queries
{
    public class GetDiscountQuery : IRequest<CouponModel>
    {
        public string ProductName { get; set; } = string.Empty;
        public GetDiscountQuery(string productName)
        {
            ProductName = productName;
        }
    }
}
