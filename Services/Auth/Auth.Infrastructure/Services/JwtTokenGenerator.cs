using Auth.Application.Common;
using Auth.Infrastructure.Entities;
using Auth.Infrastructure.Interfaces;
using Microsoft.Extensions.Options;

namespace Auth.Infrastructure.Services
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly JwtSettings _jwtSettings;
        public JwtTokenGenerator(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }
        public TokenResponse GenerateTokenPair(User user, IEnumerable<string>? permissions)
        {
            return new TokenResponse
            {
                AccessToken = GenerateAccessToken(user, permissions),
                RefreshToken = GenerateRefreshToken(),
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            };
        }

        public string GenerateAccessToken(User user, IEnumerable<string>? permissions)
        {
            throw new NotImplementedException();
        }

        public string GenerateRefreshToken()
        {
            throw new NotImplementedException();
        }


        public string HashToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
