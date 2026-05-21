using FluentValidation;

namespace Ordering.Application.Features.Commands.CheckoutOrderV2
{
    public class CheckoutOrderV2Validator : AbstractValidator<CheckoutOrderCommandV2>
    {
        public CheckoutOrderV2Validator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");


            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");

        }
    }
}
