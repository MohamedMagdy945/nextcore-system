using Auth.Application.Bases;
using MediatR;

namespace Auth.Application.Features.Auth.Logout
{
    public class LogoutCommand : IRequest<Result<bool>>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
