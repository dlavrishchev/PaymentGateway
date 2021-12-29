using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using PaymentGateway.Billing.Application.Exceptions;

namespace PaymentGateway.Billing.Application.Behaviors;

[UsedImplicitly]
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if(validationResult.IsValid)
            return await next();

        var error = validationResult.Errors[0];
        throw new BadRequestException(error.ErrorMessage, error.ErrorCode);
    }
}