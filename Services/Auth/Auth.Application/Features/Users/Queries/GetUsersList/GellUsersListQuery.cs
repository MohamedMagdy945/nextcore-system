using Auth.Application.Bases;
using Auth.Application.DTOs;
using Auth.Application.Pagination;
using MediatR;

namespace Auth.Application.Features.Users.Queries.GetUsersList
{
    public record GetUsersListQuery(PaginationParams PageParams) :
        IRequest<Result<PagedList<UserDto>>>;
}
