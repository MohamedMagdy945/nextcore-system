using Auth.Application.Common;
using Auth.Infrastructure.Entities;

namespace Auth.Infrastructure.Interfaces
{
    public interface ITokenGenerator
    {
        TokenResponse GenerateTokenPair(User user, IEnumerable<string>? permissions);
        string GenerateAccessToken(User user, IEnumerable<string>? permissions);
        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
