using Auth.Application.Bases;
using Auth.Application.DTOs;
using Auth.Application.Features.Users.Commands.AddUser;
using Auth.Application.Features.Users.Queries.GetUsersList;
using Auth.Application.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : AppControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(Result<int>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateUser(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var response = await Mediator.Send(command, cancellationToken);
            return ApiResult(response);
        }
        [HttpGet]
        [ProducesResponseType(typeof(Result<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersList([FromQuery] PaginationParams pagination, CancellationToken cancellationToken)
        {
            var query = new GetUsersListQuery(pagination);

            var response = await Mediator.Send(query, cancellationToken);

            return ApiResult(response);
        }

        [HttpGet("/{Id}")]
        [ProducesResponseType(typeof(Result<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserById(int Id)
        {

        }
    }
}