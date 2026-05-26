using Auth.Application.Features.Users.Commands.AddUser;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AppControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateUser(AddUserCommand command, CancellationToken cancellationToken)
        {

            var response = await Mediator.Send(command, cancellationToken);
            return Result(response);
        }

    }
}
