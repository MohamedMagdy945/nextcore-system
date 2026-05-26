using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Features.Auth.Register
{
    public class RegisterCommandHandler :
        IRequestHandler<RegisterCommand, Result<TokenResponse>>
    {

        private readonly IAuthService _authService;
        private readonly ILogger<RegisterCommandHandler> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        public RegisterCommandHandler(
            IAuthService authService,
            ILogger<RegisterCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _authService = authService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Result<TokenResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var registerRequest = request.Adapt<RegisterRequest>();

            var tokenResponseResult = await _authService.RegisterAsync(registerRequest, cancellationToken);

            if (!tokenResponseResult.IsSuccess)
            {
                _logger.LogWarning("Registration failed for Email: {Email}. Reason: {ErrorMessage}",
                    request.Email, tokenResponseResult.Message);
            }
            _logger.LogInformation("User with Email: {Email} registered successfully.", request.Email);

            return tokenResponseResult;
        }
    }
}
