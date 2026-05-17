using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Exceptions;
using Ordering.Application.Features.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Handlers.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        public UpdateOrderCommandHandler(
            ILogger<UpdateOrderCommandHandler> logger,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(request.Id);

            if (orderToUpdate == null)
            {
                _logger.LogError($"Order with id {request.Id} is not found.");
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }

            var orderEntity = request.Adapt(orderToUpdate);

            await _orderRepository.UpdateAsync(orderEntity);
            return Unit.Value;
        }

    }
}
