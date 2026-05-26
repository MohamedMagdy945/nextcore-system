using Auth.Application.Bases;
using FluentValidation;
using MediatR;

namespace Auth.Infrastructure.Behavior
{
    public class ValidationBehavior<TRequest, TResponse>
     : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var failures = (await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                ))
                .SelectMany(x => x.Errors)
                .Where(e => e != null)
                .Select(e => e.ErrorMessage)
                .ToList();

            if (failures.Any())
            {

                return (TResponse)(object)new Result<object>
                {
                    IsSuccess = false,
                    Errors = failures,
                    Data = null
                };

            }

            return await next();
        }
    }
}
