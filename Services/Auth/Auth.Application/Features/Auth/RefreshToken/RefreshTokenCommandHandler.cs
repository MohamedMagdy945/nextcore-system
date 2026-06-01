using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<TokenResponse>>
    {
        private readonly IClientInfoProvider _clientInfoProvider;
        private readonly IAuthService _authService;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IClientInfoProvider clientInfoProvider, IAuthService authService, ILogger<RefreshTokenCommandHandler> logger)
        {
            _clientInfoProvider = clientInfoProvider;
            _authService = authService;
            _logger = logger;
        }


        public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = request.RefreshToken,
                IpAddress = _clientInfoProvider.GetIpAddress(),
                DeviceInfo = _clientInfoProvider.GetUserAgent()
            };

            var result = await _authService.RefreshTokenAsync(refreshTokenRequest, cancellationToken);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Refresh token failed. Reason: {ErrorMessage}", result.Message);
                return result;
            }

            _logger.LogInformation("Token refreshed successfully for user: {Email}", result.Data?.Email ?? "Unknown");

            return result;
        }
    }
}
