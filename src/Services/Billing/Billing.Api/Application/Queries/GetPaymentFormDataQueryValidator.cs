using FluentValidation;
using JetBrains.Annotations;
using PaymentGateway.Billing.Application.Extensions;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Application.Queries;

[UsedImplicitly]
public class GetPaymentFormDataQueryValidator : AbstractValidator<GetPaymentFormDataQuery>
{
    public GetPaymentFormDataQueryValidator()
    {
        CascadeMode = CascadeMode.Stop;
        var errorCode = Errors.InvalidRequest.ErrorCode;

        RuleFor(x => x.InvoiceGuid)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.GuidRequired);

        RuleFor(x => x.InvoiceGuid)
            .ValidGuid()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.GuidInvalid);
    }
}