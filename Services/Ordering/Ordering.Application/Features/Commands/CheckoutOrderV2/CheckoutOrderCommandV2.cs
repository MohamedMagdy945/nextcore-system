using MediatR;

namespace Ordering.Application.Features.Commands.CheckoutOrderV2
{
    public record CheckoutOrderCommandV2 : IRequest<int>
    {
        public string? UserName { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
