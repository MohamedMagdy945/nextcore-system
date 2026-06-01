using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Auth.Register
{
    public class RegisterCommandHandler :
        IRequestHandler<RegisterCommand, Result<TokenResponse>>
    {

        private readonly IAuthService _authService;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private IClientInfoProvider _clientInfoProvider;
        public RegisterCommandHandler(
            IAuthService authService,
            ILogger<RegisterCommandHandler> logger,
            IClientInfoProvider clientInfoProvider,
            IAuthDbContext authDbContext
            )
        {
            _authService = authService;
            _logger = logger;
            _clientInfoProvider = clientInfoProvider;
        }

        public async Task<Result<TokenResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            var registerRequest = request.Adapt<RegisterRequest>();

            registerRequest.IpAddress = _clientInfoProvider.GetIpAddress();
            registerRequest.DeviceInfo = _clientInfoProvider.GetUserAgent();

            var result = await _authService.RegisterAsync(registerRequest, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Registration failed for Email: {Email}. Reason: {ErrorMessage}",
                    request.Email, result.Message);
                return result;
            }
            _logger.LogInformation("User with Email: {Email} registered successfully.", request.Email);

            return result;
        }
    }
}
