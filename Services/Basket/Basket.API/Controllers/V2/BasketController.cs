using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Core.Entities;
using Catalog.API.Controllers;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
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
            _logger.LogInformation($"Basket Published for {basket.UserName} v2 endpoint");
            var deleteCommand = new DeleteBasketByUserNameCommand(basket.UserName);
            await _mediator.Send(deleteCommand);
            return Accepted();
        }
    }
}
