using Asp.Versioning;
using Basket.Application.Features.Commands.DeleteShoppingCartByUserName;
using Basket.Application.Features.Queries;
using Basket.Core.Entities;
using Catalog.API.Controllers;
using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    public class BasketController : AppControllerBase
    {

        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<BasketController> _logger;
        public BasketController(
            IPublishEndpoint publishEndpoint,
            ILogger<BasketController> logger
            )
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;

        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await Mediator.Send(query);

            if (basket == null)
            {
                return BadRequest();
            }

            var eventMsg = basketCheckout.Adapt<BasketCheckoutEventV2>();
            eventMsg.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMsg);
            _logger.LogInformation($"Basket Published for {basket.UserName} v2 endpoint");
            var deleteCommand = new DeleteShoppingCartByUserNameCommand(basket.UserName);
            await Mediator.Send(deleteCommand);
            return Accepted();
        }
    }
}