using Auth.Application.Bases;
using MediatR;

namespace Auth.Application.Features.Auth.Login
{
    public class LoginCommand : IRequest<Result<Unit>>
    {
    }
}
