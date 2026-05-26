using Auth.Application.Bases;
using Auth.Application.DTOs;
using Auth.Application.Interfaces;
using Auth.Application.Pagination;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Features.Users.Queries.GetUsersList
{
    public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, Result<PagedList<UserDto>>>
    {
        private readonly IAuthDbContext _context;

        public GetUsersListQueryHandler(IAuthDbContext context) => _context = context;

        public async Task<Result<PagedList<UserDto>>> Handle(GetUsersListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Users
                .AsNoTracking().OrderBy(u => u.Id)
                .Select(u => new UserDto
                {
                    UserName = u.UserName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt,
                    ImageUrl = u.ImageUrl,
                    IsEnabled = u.IsEnabled,
                });

            var pagedUsers = await PagedList<UserDto>.CreateAsync(
                query,
                request.PageParams.PageNumber,
                request.PageParams.PageSize
            );

            return Result<PagedList<UserDto>>.Success(pagedUsers);
        }
    }
}
