using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Features.Commands.CheckoutOrderV2
{
    public class CheckoutOrderV2Handler : IRequestHandler<CheckoutOrderCommandV2, int>
    {
        private readonly ILogger<CheckoutOrderV2Handler> _logger;

        private readonly IOrderRepository _orderRepository;
        public CheckoutOrderV2Handler(
            ILogger<CheckoutOrderV2Handler> logger,

            IOrderRepository orderRepository)
        {
            _logger = logger;

            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CheckoutOrderCommandV2 request, CancellationToken cancellationToken)
        {
            var orderEntity = request.Adapt<Order>();
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is successfully created wit v2 handler.", newOrder.Id);
            return newOrder.Id;
        }
    }
}
