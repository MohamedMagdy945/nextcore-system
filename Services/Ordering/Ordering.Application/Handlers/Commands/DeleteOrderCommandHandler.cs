using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }


        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new OrderNotFoundException($"Order with name {nameof(order)} not found.", request.Id);

            }

            await _orderRepository.DeleteAsync(order);
            _logger.LogInformation($"Order with id {order.Id} deleted.", request.Id);
            return Unit.Value;
        }
    }
}
