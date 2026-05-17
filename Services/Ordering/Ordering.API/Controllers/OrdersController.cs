using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Features.Commands;
using Ordering.Application.Features.Commands.CheckoutOrder;
using Ordering.Application.Features.Commands.UpdateOrder;
using Ordering.Application.Features.Queries.GetOrdersByUserName;
using Ordering.Application.Responses;

namespace Ordering.API.Controllers
{
    public class OrdersController : AppControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{userName}", Name = "GetOrdersByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersByUserName(string userName)
        {
            var query = new GetOrdersByUserNameQuery(userName);
            var orders = await Mediator.Send(query);
            return Ok(orders);
        }

        [HttpPost("CheckoutOrder")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        public async Task<ActionResult> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);

        }

        [HttpPut("UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var result = await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("DeleteOrder/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await Mediator.Send(new DeleteOrderCommand(id));
            return NoContent();

        }
    }
}
