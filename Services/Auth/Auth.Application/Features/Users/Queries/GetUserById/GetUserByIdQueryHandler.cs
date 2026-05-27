using Auth.Application.Bases;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Mapster;
using MediatR;

namespace Auth.Application.Features.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserById, Result<UserDto>>
    {
        private readonly IAuthDbContext _authDbContext;

        public GetUserByIdQueryHandler(IAuthDbContext authDbContext)
        {
            _authDbContext = authDbContext;
        }
        public async Task<Result<UserDto>> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            var user = await _authDbContext.Users.FindAsync(request.Id, cancellationToken);

            if (user == null)
                return Result<UserDto>.NotFound($"User with ID {request.Id} not found.");

            var userDto = user.Adapt<UserDto>();

            return Result<UserDto>.Success(userDto, "User retrieved successfully.");
        }
    }
}
