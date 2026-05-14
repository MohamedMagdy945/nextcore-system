using Discount.Application.Features.Commands;
using Discount.Core.Repositories;
using MediatR;

namespace Discount.Application.Features.Handlers.Commands
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _discountRepository;

        public DeleteDiscountCommandHandler(IDiscountRepository discountRepository)
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
