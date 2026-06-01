using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Features.Auth.Register;
using Auth.Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Auth.Login
{
    public class LoginCommandHandler :
        IRequestHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IAuthService _authService;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private IClientInfoProvider _clientInfoProvider;
        public LoginCommandHandler(IAuthService authService,
            ILogger<RegisterCommandHandler> logger, IClientInfoProvider clientInfoProvider)
        {
            _authService = authService;
            _logger = logger;
            _clientInfoProvider = clientInfoProvider;
        }

        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var loginRequest = request.Adapt<LoginRequest>();
            loginRequest.IpAddress = _clientInfoProvider.GetIpAddress();
            loginRequest.DeviceInfo = _clientInfoProvider.GetUserAgent();

            var result = await _authService.LoginAsync(loginRequest, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Login failed for Email: {Email}. Reason: {ErrorMessage}",
                    request.Email, result.Message);

                return result;
            }
            _logger.LogInformation("User with Email: {Email} logged in successfully.", request.Email);

            return result;
        }
    }
}
