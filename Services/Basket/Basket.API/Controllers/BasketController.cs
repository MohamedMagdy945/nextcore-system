using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using Catalog.API.Controllers;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
    [ApiVersion("1")]
    public class BasketController : BaseApiController
    {

        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;
        private readonly ILogger<BasketController> _logger;
        public BasketController(
            IPublishEndpoint publishEndpoint,
            IMapper mapper,
            ILogger<BasketController> logger
            )
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]/{userName}", Name = "GetBasketByUserName")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> GetBasket(string userName)
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCartResponse>> UpdateBasket([FromBody] CreateShoppingCartCommand command)
        {
            var basket = await _mediator.Send(command);
            return Ok(basket);
        }

        [HttpDelete()]
        [Route("[action]/{userName}", Name = "DeleteBasketByUserName")]
        public async Task<ActionResult<ShoppingCartResponse>> DeleteBasket(string userName)
        {
            var command = new DeleteBasketByUserNameCommand(userName);
            var basket = await _mediator.Send(command);
            return Ok(basket);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout(BasketCheckout basketCheckout)
        {
            var query = new GetBasketByUserNameQuery(basketCheckout.UserName);
            var basket = await _mediator.Send(query);

            if (basket == null)
            {
                return BadRequest();
            }

            var eventMsg = _mapper.Map<BasketCheckoutEventV2>(basketCheckout);
            eventMsg.TotalPrice = basket.TotalPrice;
            await _publishEndpoint.Publish(eventMsg);
            _logger.LogInformation($"Basket Published for {basket.UserName}");
            var deleteCommand = new DeleteBasketByUserNameCommand(basket.UserName);
            await _mediator.Send(deleteCommand);
            return Accepted();
        }
    }
}
