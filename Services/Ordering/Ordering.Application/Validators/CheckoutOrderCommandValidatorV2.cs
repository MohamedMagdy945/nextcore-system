using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidatorV2 : AbstractValidator<CheckoutOrderCommadV2>
    {
        public CheckoutOrderCommandValidatorV2()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");
        }
    }
}
