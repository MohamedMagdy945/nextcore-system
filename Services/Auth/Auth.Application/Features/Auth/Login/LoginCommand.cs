using Auth.Application.Bases;
using Auth.Application.Common;
using MediatR;

namespace Auth.Application.Features.Auth.Login
{
    public record LoginCommand : IRequest<Result<TokenResponse>>
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
