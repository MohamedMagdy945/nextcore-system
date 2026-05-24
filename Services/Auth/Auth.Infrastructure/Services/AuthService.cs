using Auth.Application.Bases;
using Auth.Application.Common;
using Auth.Application.Interfaces;

namespace Auth.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        public Task<Result<TokenResponse>> LoginAsync(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<Result<LogoutResponse>> LogoutAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<Result<TokenResponse>> RegisterAsync(string username, string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
