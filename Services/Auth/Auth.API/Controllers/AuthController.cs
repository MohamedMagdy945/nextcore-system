using Auth.API.Helpers;
using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.Features.Auth.Login;
using Auth.Application.Features.Auth.RefreshToken;
using Auth.Application.Features.Auth.Register;
using Mapster;
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
        [ProducesResponseType(typeof(Result<AccessTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var response = await Mediator.Send(command);

            if (!response.IsSuccess || response.Data is null)
                return ApiResponse(response);

            CookieHelper.SetRefreshTokenCookie(
                Response,
                response.Data.RefreshToken,
                response.Data.RefreshTokenExpiration,
                HttpContext.Request.IsHttps
            );


            var accessTokenResponse = response.Data.Adapt<AccessTokenResponse>();

            var result = Result<AccessTokenResponse>.Success(accessTokenResponse);

            return ApiResponse(result);
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(Result<AccessTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await Mediator.Send(command);

            if (!response.IsSuccess || response.Data is null)
                return ApiResponse(response);

            CookieHelper.SetRefreshTokenCookie(
                Response,
                response.Data.RefreshToken,
                response.Data.RefreshTokenExpiration,
                HttpContext.Request.IsHttps
            );


            var accessTokenResponse = response.Data.Adapt<AccessTokenResponse>();

            var result = Result<AccessTokenResponse>.Success(accessTokenResponse);

            return ApiResponse(result);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(Result<AccessTokenResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshTokenString = Request.Cookies["refreshToken"];

            var command = new RefreshTokenCommand
            {
                RefreshToken = refreshTokenString
            };


            var response = await Mediator.Send(command);

            if (!response.IsSuccess || response.Data is null)
                return ApiResponse(response);

            CookieHelper.SetRefreshTokenCookie(
                Response,
                response.Data.RefreshToken,
                response.Data.RefreshTokenExpiration,
                HttpContext.Request.IsHttps
            );


            var accessTokenResponse = response.Data.Adapt<AccessTokenResponse>();

            var result = Result<AccessTokenResponse>.Success(accessTokenResponse);

            return ApiResponse(result);
        }

    }
}
