using Auth.Application.Bases;
using MediatR;

namespace Auth.Application.Features.Users.Commands.AddUser
{
    public record CreateUserCommand : IRequest<Result<int>>
    {
        public required string UserName { get; init; }
        public required string Email { get; init; }
    }
}
