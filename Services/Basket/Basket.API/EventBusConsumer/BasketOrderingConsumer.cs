using Basket.Application.Features.Commands.CreateShoppingCart;
using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using MediatR;

namespace Basket.API.EventBusConsumer
{
    public class BasketOrderingConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BasketOrderingConsumer> _logger;
        public BasketOrderingConsumer(IMediator mediator,
            ILogger<BasketOrderingConsumer> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            var message = context.Message;

            using var scope = _logger.BeginScope(
                "Consume BasketCheckoutEvent - CorrelationId: {CorrelationId}",
                message.CorrelationId);

            var cmd = message.Adapt<CreateShoppingCartCommand>();

            var result = await _mediator.Send(cmd);

            _logger.LogInformation(
                "BasketCheckoutEvent consumed successfully for User: {UserName}",
                message.UserName);
        }
    }
}
