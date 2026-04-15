using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .GreaterThan(0).WithMessage("Id must be greater than zero.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("UserName is required.")
                .MaximumLength(50).WithMessage("UserName must not exceed 50 characters.");

            RuleFor(x => x.EmailAddress)
                .NotEmpty().WithMessage("EmailAddress is required.")
                .EmailAddress().WithMessage("EmailAddress must be a valid email format.");

            RuleFor(x => x.TotalPrice)
                .GreaterThan(0).WithMessage("TotalPrice must be greater than zero.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("FirstName is required.")
                .MaximumLength(50).WithMessage("FirstName must not exceed 50 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("LastName is required.")
                .MaximumLength(50).WithMessage("LastName must not exceed 50 characters.");
        }

    }
}
