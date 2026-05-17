using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
namespace Ordering.Application.Handlers.Commands
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;

        private readonly IOrderRepository _orderRepository;
        public CheckoutOrderCommandHandler(
            ILogger<CheckoutOrderCommandHandler> logger,

            IOrderRepository orderRepository)
        {
            _logger = logger;

            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = request.Adapt<Order>();
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is successfully created.", newOrder.Id);
            return newOrder.Id;
        }
    }
}
