using FluentValidation;
using JetBrains.Annotations;
using PaymentGateway.Billing.Application.Extensions;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Application.Commands.Invoices;

[UsedImplicitly]
public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        CascadeMode = CascadeMode.Stop;
        var errorCode = Errors.InvalidRequest.ErrorCode;

        RuleFor(x => x.ShopId)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Shop.IdRequired);
        RuleFor(x => x.ShopId)
            .ValidMongoId()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Shop.IdInvalid);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.AmountInvalid);

        RuleFor(x => x.Currency)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.CurrencyRequired);
        RuleFor(x => x.Currency)
            .Matches("^[A-Z]{3}$")
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.CurrencyInvalid);

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.DescriptionRequired);
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.Invoice.DescriptionTooLong);
    }
}