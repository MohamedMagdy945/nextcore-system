using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Features.Commands.DeleteDiscount
{
    public record DeleteDiscountCommand(string ProductName) : IRequest<bool>;
    public class DeleteDiscountHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;

        public DeleteDiscountHandler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _discountRepository.DeleteDiscountAsync(request.ProductName);
            return deleted;
        }
    }
}
