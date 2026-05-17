using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using MediatR;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketOrderingConsumer> _logger;
        public BasketOrderingConsumer(IMediator mediator,
            ILogger<BasketOrderingConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _logger.BeginScope("consuming basket checkout event for {correlationid}", context.Message.CorrelationId);
            var cmd = context.Message.Adapt<BasketCheckoutEventV2>();
            var result = await _mediator.Send(cmd);
            _logger.LogInformation("Basket checkout event completed!!");
        }
    }
}
