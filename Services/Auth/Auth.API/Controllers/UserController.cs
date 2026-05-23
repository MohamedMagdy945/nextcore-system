using Auth.Application.Features.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{

    public class UserController : AppControllerBase
    {

        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            _logger.LogInformation("Login attempt for user");

            var response = await Mediator.Send(command);
            return Result(response);
        }

    }
}
