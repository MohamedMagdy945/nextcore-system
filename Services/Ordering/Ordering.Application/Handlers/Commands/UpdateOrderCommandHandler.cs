using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers.Commands
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        public UpdateOrderCommandHandler(
            ILogger<UpdateOrderCommandHandler> logger,
            IMapper mapper,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _mapper = mapper;
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

            var orderEntity = _mapper.Map(request, orderToUpdate);

            await _orderRepository.UpdateAsync(orderEntity);
            return Unit.Value;
        }

    }
}
