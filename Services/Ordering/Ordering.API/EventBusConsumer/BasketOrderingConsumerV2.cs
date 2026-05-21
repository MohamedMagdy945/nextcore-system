using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using MediatR;
using Ordering.Application.Features.Commands.CheckoutOrderV2;

namespace Ordering.API.EventBusConsumer
{
    public class BasketOrderingConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketOrderingConsumerV2> _logger;
        public BasketOrderingConsumerV2(IMediator mediator,
            ILogger<BasketOrderingConsumerV2> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            var message = context.Message;

            using var scope = _logger.BeginScope(
                "Consume BasketCheckoutEvent - CorrelationId: {CorrelationId}",
                message.CorrelationId);

            var cmd = message.Adapt<CheckoutOrderCommandV2>();

            var result = await _mediator.Send(cmd);

            _logger.LogInformation(
                "BasketCheckoutEvent consumed successfully for User: {UserName}",
                message.UserName);
        }
    }
}
