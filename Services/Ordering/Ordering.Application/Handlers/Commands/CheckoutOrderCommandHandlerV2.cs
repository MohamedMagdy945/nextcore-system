using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands
{
    public class CheckoutOrderCommandHandlerV2 : IRequest<CheckoutOrderCommandV2>
    {
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        private readonly IOrderRepository _orderRepository;
        public CheckoutOrderCommandHandlerV2(
            ILogger<CheckoutOrderCommandHandler> logger,

            IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CheckoutOrderCommandV2 request, CancellationToken cancellationToken)
        {
            var orderEntity = request.Adapt<Order>();
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is successfully created. with version 2 handler", newOrder.Id);
            return newOrder.Id;
        }
    }
}
