using Auth.Application.Common;
using Auth.Domain.Entities;
using Auth.Infrastructure.Interfaces;
using Auth.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
                UserId = user.Id,
                Email = user.Email,
                AccessToken = GenerateAccessToken(user, permissions),
                RefreshToken = GenerateRefreshToken(),
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays),
            };
        }

        public string GenerateAccessToken(User user, IEnumerable<string>? permissions)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (permissions != null && permissions.Any())
            {
                claims.AddRange(permissions!.Select(p =>
                         new Claim("permission", p)));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.AccessTokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
               issuer: _jwtSettings.Issuer,
               audience: _jwtSettings.Audience,
               claims: claims,
               expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
               signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


        public string HashToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.RefreshTokenSecret);

            using var hmac = new HMACSHA256(key);
            var bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(token));

            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
