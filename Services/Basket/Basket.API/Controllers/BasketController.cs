using Asp.Versioning;
using Basket.Application.Features.Commands.CreateShoppingCart;
using Basket.Application.Features.Commands.DeleteShoppingCartByUserName;
using Basket.Application.Features.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Catalog.API.Controllers;
using EventBus.Messages.Events;
using Mapster;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiVersion("1.0")]
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


        [HttpGet]
        [Route("[action]/{email}", Name = "GetBasketByEmail")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string email)
        {
            var query = new GetBasketByEmailQuery(email);
            var basket = await Mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var basket = await Mediator.Send(command);
            return Ok(basket);
        }

        [HttpDelete()]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
        {
            var command = new DeleteShoppingCartByUserNameCommand(userName);
            var basket = await Mediator.Send(command);
            return Ok(basket);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var query = new GetBasketByEmailQuery(basketCheckout.UserName);
            var basket = await Mediator.Send(query);

            if (basket == null)
            {
                return BadRequest();
            }

            var eventMsg = basketCheckout.Adapt<BasketCheckoutEvent>();
            eventMsg.TotalPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMsg);

            _logger.LogInformation($"Basket Published for {basket.Email}");
            var deleteCommand = new DeleteShoppingCartByUserNameCommand(basket.Email);
            await Mediator.Send(deleteCommand);
            return Accepted();
        }
    }
}
