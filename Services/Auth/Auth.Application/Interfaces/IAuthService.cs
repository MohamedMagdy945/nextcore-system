using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.DTOs;

namespace Auth.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<TokenResponse>> RegisterAsync(RegisterRequest request);
        Task<Result<TokenResponse>> LoginAsync(string username, string password);
        Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken);
        Task<Result<LogoutResponse>> LogoutAsync(string refreshToken);

    }
}
