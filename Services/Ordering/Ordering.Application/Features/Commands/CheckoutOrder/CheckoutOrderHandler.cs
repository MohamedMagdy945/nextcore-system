using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Commands.CheckoutOrder
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly ILogger<CheckoutOrderHandler> _logger;

        private readonly IOrderRepository _orderRepository;
        public CheckoutOrderHandler(
            ILogger<CheckoutOrderHandler> logger,

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
