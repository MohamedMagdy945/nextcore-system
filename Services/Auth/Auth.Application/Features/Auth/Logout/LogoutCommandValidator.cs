using FluentValidation;

namespace Auth.Application.Features.Auth.Logout
{
    public class LogoutCommandValidator
        : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required");
        }

    }
}
