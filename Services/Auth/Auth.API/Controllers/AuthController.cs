using Auth.Application.Features.Auth.Login;
using Auth.Application.Features.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{

    public class AuthController : AppControllerBase
    {

        private readonly ILogger<AuthController> _logger;
        public AuthController(ILogger<AuthController> logger)
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await Mediator.Send(command);
            return Result(response);
        }

    }
}
