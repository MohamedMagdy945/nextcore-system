using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;

namespace Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
        Task<Result<TokenResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<Result<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken);
        Task<Result<LogoutResponse>> LogoutAsync(string refreshToken);
    }
}
