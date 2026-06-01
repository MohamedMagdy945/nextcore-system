using Auth.Application.Bases;
using Auth.Application.DTOs;
using Auth.Application.Features.Auth.RefreshToken;
using Auth.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Auth.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<bool>>
    {

        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly IAuthService _authService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public LogoutCommandHandler(IClientInfoProvider clientInfoProvider, IAuthService authService, ILogger<RefreshTokenCommandHandler> logger)
        {
            _clientInfoProvider = clientInfoProvider;
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var logoutRequest = new LogoutRequest
            {
                RefreshToken = request.RefreshToken,
                IpAddress = _clientInfoProvider.GetIpAddress(),
                DeviceInfo = _clientInfoProvider.GetUserAgent()
            };

            var result = await _authService.LogoutAsync(logoutRequest, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Logout failed. Reason: {ErrorMessage}", result.Message);
                return result;
            }
            return result;
        }
    }
}
