using FluentValidation;
using JetBrains.Annotations;
using PaymentGateway.Billing.Application.Extensions;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Application.Commands.Payments;

[UsedImplicitly]
public class ProcessPaymentCommandValidator : AbstractValidator<ProcessPaymentCommand>
{
    public ProcessPaymentCommandValidator(IDateTimeProvider dateTimeProvider)
    {
        CascadeMode = CascadeMode.Stop;
        var errorCode = Errors.InvalidRequest.ErrorCode;

        RuleFor(command => command.InvoiceGuid)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.GuidRequired);
        RuleFor(command => command.InvoiceGuid)
            .ValidGuid()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.GuidInvalid);

        RuleFor(command => command.PaymentMethodId)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.PaymentMethod.Required);
        RuleFor(command => command.PaymentMethodId)
            .ValidMongoId()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.PaymentMethod.Invalid);

        RuleFor(command => command.BankCardData).SetValidator(new BankCardValidator(dateTimeProvider));
    }
}