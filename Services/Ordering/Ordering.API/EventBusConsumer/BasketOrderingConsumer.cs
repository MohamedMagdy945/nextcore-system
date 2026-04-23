using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketOrderingConsumer> _logger;
        public BasketOrderingConsumer(IMediator mediator,
            IMapper mapper,
            ILogger<BasketOrderingConsumer> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope("consuming basket checkout event for {correlationid}", context.Message.CorrelationId);
            var cmd = _mapper.Map<BasketCheckoutEventV2>(context.Message);
            var result = await _mediator.Send(cmd);
            _logger.LogInformation("Basket checkout event completed!!");
        }
    }
}
