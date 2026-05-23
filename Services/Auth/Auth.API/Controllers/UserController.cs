using Auth.Application.Features.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{

    public class UserController : AppControllerBase
    {

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {

            var response = await Mediator.Send(command);
            return Result(response);
        }

    }
}
