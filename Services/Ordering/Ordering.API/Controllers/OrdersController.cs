using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Responses;

namespace Ordering.API.Controllers
{
    public class OrdersController : BaseApiController
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
            var query = new GetOrdersListQuery(userName);
            var orders = await _mediator.Send(query);
            return Ok(orders);
        }

    }
}
