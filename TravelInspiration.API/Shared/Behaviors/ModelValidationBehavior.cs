using FluentValidation;
using MediatR;

namespace TravelInspiration.API.Shared.Behaviors;

public class ModelValidationBehavior<TRequest, TResult>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResult> Handle(
        TRequest request,
        RequestHandlerDelegate<TResult> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var failures = _validators
            .Select(x => x.Validate(context))
            .SelectMany(x => x.Errors)
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());

        if (failures.Count == 0)
            return await next();

        return (TResult)Results.ValidationProblem(failures);
    }
}
