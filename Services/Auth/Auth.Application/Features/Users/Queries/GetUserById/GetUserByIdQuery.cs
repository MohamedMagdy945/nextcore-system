using Auth.Application.Bases;
using Auth.Application.DTOs;
using MediatR;

namespace Auth.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery : IRequest<Result<UserDto>>
    {
        public int Id { get; set; }
    }
}
