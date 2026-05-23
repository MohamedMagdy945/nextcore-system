using Auth.Application.Bases;
using Auth.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Auth.Application.Features.Auth.Login
{
    public class LoginCommandHandler :
        IRequestHandler<LoginCommand, Response<Unit>>
    {
        private readonly IStringLocalizer<AuthSharedResource> _localizer;

        public LoginCommandHandler(IStringLocalizer<AuthSharedResource> localizer)
        {
            _localizer = localizer;
        }

        public async Task<Response<Unit>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            bool userExists = false;

            if (!userExists)
            {
                return Response<Unit>.Failure(Messages.UserNotFound);
            }

            return Response<Unit>.Success(Unit.Value);
        }
    }
}
