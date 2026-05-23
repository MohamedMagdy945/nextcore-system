using Auth.Application.Resources;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Auth.Application.Features.Auth.Login
{
    public class LoginCommandHandler // : IRequestHandler<LoginCommand, AuthResult>
    {
        private readonly IStringLocalizer<AuthSharedResource> _localizer;

        public LoginCommandHandler(IStringLocalizer<AuthSharedResource> localizer)
        {
            _localizer = localizer;
        }

        public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // فرضاً بنعمل تشيك على المستخدم وموجودش في الـ Database
            bool userExists = false;

            if (!userExists)
            {
                string errorMessage = _localizer["UserNotFound"];

                throw new Exception(errorMessage);
            }

            return Unit.Value;
        }
    }
}
