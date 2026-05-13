using Basket.Core.Repositories;
using MediatR;

namespace Basket.Application.Features.Commands.DeleteShoppingCartByUserName
{
    public record class DeleteShoppingCartByUserNameCommand(string UserName)
        : IRequest<Unit>;
    public class DeleteShoppingCartByUserNameHandler
        : IRequestHandler<DeleteShoppingCartByUserNameCommand, Unit>
    {
        private readonly IBasketRepository _basketRepository;

        public DeleteShoppingCartByUserNameHandler(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        public async Task<Unit> Handle(DeleteShoppingCartByUserNameCommand request, CancellationToken cancellationToken)
        {
            await _basketRepository.DeleteCartAsync(request.UserName);
            return Unit.Value;
        }
    }

}
