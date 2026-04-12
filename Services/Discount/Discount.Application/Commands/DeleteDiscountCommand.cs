using MediatR;

namespace Discount.Application.Commands
{
    public class DeleteDiscountCommand : IRequest<bool>
    {
        public string ProductName { get; set; } = string.Empty;
        public DeleteDiscountCommand(string productName)
        {

            ProductName = productName;
        }
    }
}
