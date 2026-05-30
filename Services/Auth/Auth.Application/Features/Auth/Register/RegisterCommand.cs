using Auth.Application.Bases;
using Auth.Application.Common;
using MediatR;

namespace Auth.Application.Features.Auth.Register
{
    public record RegisterCommand :
        IRequest<Result<TokenResponse>>
    {
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
