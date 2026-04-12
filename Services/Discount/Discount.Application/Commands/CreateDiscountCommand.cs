using Discount.Grpc.Protos;
using MediatR;

namespace Discount.Application.Commands
{
    public class CreateDiscountCommand : IRequest<CouponModel>
    {
        public string ProductName { get; set; } = string.Empty;
        public string Discription { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
}
