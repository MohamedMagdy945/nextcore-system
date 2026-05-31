using System.Security.Cryptography;
using System.Text;

namespace Auth.Infrastructure.Services
{
    public class RefreshTokenService
    {
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
