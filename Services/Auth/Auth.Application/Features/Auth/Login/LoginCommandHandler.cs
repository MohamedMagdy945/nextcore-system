using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Features.Auth.Register;
using Auth.Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Auth.Login
{
    public class LoginCommandHandler :
        IRequestHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public LoginCommandHandler(IAuthService authService,
            ILogger<RegisterCommandHandler> logger, IHttpContextAccessor httpContextAccessor)
        {
            _authService = authService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginRequest = request.Adapt<LoginRequest>();
            loginRequest.IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            loginRequest.DeviceInfo = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();

            var result = await _authService.LoginAsync(loginRequest, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Login failed for Email: {Email}. Reason: {ErrorMessage}",
                    request.Email, result.Message);
            }
            _logger.LogInformation("User with Email: {Email} logged in successfully.", request.Email);

            return result;
        }
    }
}
