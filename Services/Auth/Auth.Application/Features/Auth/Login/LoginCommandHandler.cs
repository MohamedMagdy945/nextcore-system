using Auth.Application.Bases;
using Auth.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Auth.Application.Features.Auth.Login
{
    public class LoginCommandHandler :
        IRequestHandler<LoginCommand, Result<Unit>>
    {
        private readonly IStringLocalizer<AuthSharedResource> _localizer;

        public LoginCommandHandler(IStringLocalizer<AuthSharedResource> localizer)
        {
            _localizer = localizer;
        }

        public async Task<Result<Unit>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            bool userExists = false;

            if (!userExists)
            {
                return Result<Unit>.Failure(Messages.UserNotFound);
            }

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
