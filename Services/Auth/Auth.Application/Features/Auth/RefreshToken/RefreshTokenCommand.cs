using Auth.Application.Bases;
using Auth.Application.Common;
using MediatR;

namespace Auth.Application.Features.Auth.RefreshToken
{
    public class RefreshTokenCommand : IRequest<Result<TokenResponse>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
