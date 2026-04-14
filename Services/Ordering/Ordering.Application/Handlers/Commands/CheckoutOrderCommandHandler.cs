using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
namespace Ordering.Application.Handlers.Commands
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        public CheckoutOrderCommandHandler(
            ILogger<CheckoutOrderCommandHandler> logger,
            IMapper mapper,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _orderRepository = orderRepository;
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Core.Entities.Order>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);
            _logger.LogInformation($"Order {newOrder.Id} is successfully created.", newOrder.Id);
            return newOrder.Id;
        }
    }
}
