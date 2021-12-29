using System;
using FluentValidation;
using PaymentGateway.Billing.Application.Dto;
using PaymentGateway.Billing.Application.Extensions;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Application.Commands.Payments;

public class BankCardValidator : AbstractValidator<BankCardDataDto>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    public BankCardValidator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
        CascadeMode = CascadeMode.Stop;
        var errorCode = Errors.InvalidRequest.ErrorCode;

        RuleFor(x => x.Number)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.NumberRequired);
        RuleFor(x => x.Number)
            .Matches(@"^\d{13,19}$")
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.NumberFormatInvalid);
        RuleFor(x => x.Number)
            .Must(s => BeValidNumber(s))
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.NumberInvalid);

        RuleFor(x => x.Holder)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.HolderRequired);
        RuleFor(x => x.Holder)
            .MaximumLength(50)
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.HolderTooLong);

        RuleFor(x => x.ExpiryMonth)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.ExpiryMonthRequired);
        Transform(from: x => x.ExpiryMonth, to: x => x.ToNullableInt())
            .NotNull().WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.ExpiryMonthInvalid)
            .GreaterThanOrEqualTo(1).WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.ExpiryMonthInvalid)
            .LessThanOrEqualTo(12).WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.ExpiryMonthInvalid);

        RuleFor(x => x.ExpiryYear)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.ExpiryYearRequired);
        Transform(from: x => x.ExpiryYear, to: x => x.ToNullableInt())
            .NotNull().WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.ExpiryYearInvalid)
            .GreaterThanOrEqualTo(1).WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.ExpiryYearInvalid)
            .LessThanOrEqualTo(99).WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.ExpiryYearInvalid);

        RuleFor(x => x)
            .Must(BeNotExpiredCard)
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.CardExpired);

        RuleFor(x => x.Cvv)
            .NotEmpty()
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.CvvRequired);
        RuleFor(x => x.Cvv)
            .MinimumLength(3).WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.CvvInvalid)
            .MaximumLength(4).WithErrorCode(errorCode).WithMessage(ValidationMessages.BankCard.CvvInvalid)
            .Must(cvv => int.TryParse(cvv, out var intCvv) && intCvv > 0)
            .WithErrorCode(errorCode)
            .WithMessage(ValidationMessages.BankCard.CvvInvalid);
    }

    private bool BeNotExpiredCard(BankCardDataDto cardData)
    {
        var cardMonth = int.Parse(cardData.ExpiryMonth);
        var cardYear = int.Parse(cardData.ExpiryYear);
        var now = _dateTimeProvider.Now();
        var nowMonth = now.Month;
        var nowYear = now.YearLastTwoDigits();

        if (cardYear > nowYear)
            return true;

        if (cardYear == nowYear)
        {
            return cardMonth >= nowMonth;
        }

        return false;
    }

    private bool BeValidNumber(ReadOnlySpan<char> number)
    {
        var sum = 0;
        var alt = false;

        for (var i = number.Length - 1; i >= 0; i--)
        {
            var digit = int.Parse(number.Slice(i, 1));

            if (alt)
            {
                digit *= 2;
                if (digit > 9)
                    digit = digit % 10 + 1;
            }

            sum += digit;
            alt = !alt;
        }

        return sum % 10 == 0;
    }
}