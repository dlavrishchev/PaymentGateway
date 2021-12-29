using System;
using FluentValidation.TestHelper;
using Moq;
using PaymentGateway.Billing.Application;
using PaymentGateway.Billing.Application.Commands.Payments;
using PaymentGateway.Billing.Application.Dto;
using PaymentGateway.Billing.Domain;
using Xunit;

namespace Billing.UnitTests.Application.Validators;

public class BankCardValidatorTests
{
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly BankCardValidator _validator;
    private readonly string _errorCode;

    public BankCardValidatorTests()
    {
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _validator = new BankCardValidator(_dateTimeProviderMock.Object);
        _errorCode = Errors.InvalidRequest.ErrorCode;
    }

    #region number

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_number_is_null_or_empty(string number)
    {
        var cardData = GetBankCardData();
        cardData.Number = number;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Number)
            .WithErrorMessage(ValidationMessages.BankCard.NumberRequired)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123")]
    [InlineData(" 5555555555554444")]
    [InlineData("5555555555554444 ")]
    [InlineData("5555 555555554444")]
    [InlineData("5555 5555 5555 4444")]
    [InlineData("55555555a5554444")]
    [InlineData("55555555555544447318")]
    [InlineData("555555555555")]
    public void Should_have_error_when_number_format_is_invalid(string number)
    {
        var cardData = GetBankCardData();
        cardData.Number = number;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Number)
            .WithErrorMessage(ValidationMessages.BankCard.NumberFormatInvalid)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("5555555565554449")]
    public void Should_have_error_when_number_is_invalid(string number)
    {
        var cardData = GetBankCardData();
        cardData.Number = number;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Number)
            .WithErrorMessage(ValidationMessages.BankCard.NumberInvalid)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("5555555555554444")]
    public void Should_not_have_error_when_number_is_valid(string number)
    {
        var cardData = GetBankCardData();
        cardData.Number = number;

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c.Number);
    }

    #endregion

    #region holder

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_holder_is_null_or_empty(string holder)
    {
        var cardData = GetBankCardData();
        cardData.Holder = holder;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Holder)
            .WithErrorMessage(ValidationMessages.BankCard.HolderRequired)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_have_error_when_holder_is_too_long()
    {
        var cardData = GetBankCardData();
        cardData.Holder = new string('a', 51);

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Holder)
            .WithErrorMessage(ValidationMessages.BankCard.HolderTooLong)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("holder")]
    public void Should_not_have_error_when_holder_is_valid(string holder)
    {
        var cardData = GetBankCardData();
        cardData.Holder = holder;

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c.Holder);
    }

    #endregion

    #region expiryMonth

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_expiry_month_is_null_or_empty(string expiryMonth)
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = expiryMonth;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.ExpiryMonth)
            .WithErrorMessage(ValidationMessages.BankCard.ExpiryMonthRequired)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("0")]
    [InlineData("13")]
    [InlineData("a")]
    public void Should_have_error_when_expiry_month_is_invalid(string expiryMonth)
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = expiryMonth;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.ExpiryMonth)
            .WithErrorMessage(ValidationMessages.BankCard.ExpiryMonthInvalid)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("1")]
    [InlineData("01")]
    public void Should_not_have_error_when_expiry_month_is_valid(string expiryMonth)
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = expiryMonth;

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c.ExpiryMonth);
    }

    #endregion

    #region expiryYear

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_expiry_year_is_null_or_empty(string expiryYear)
    {
        var cardData = GetBankCardData();
        cardData.ExpiryYear = expiryYear;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.ExpiryYear)
            .WithErrorMessage(ValidationMessages.BankCard.ExpiryYearRequired)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("a")]
    [InlineData("100")]
    public void Should_have_error_when_expiry_year_is_invalid(string expiryYear)
    {
        var cardData = GetBankCardData();
        cardData.ExpiryYear = expiryYear;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.ExpiryYear)
            .WithErrorMessage(ValidationMessages.BankCard.ExpiryYearInvalid)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("01")]
    public void Should_not_have_error_when_expiry_year_is_valid(string expiryYear)
    {
        var cardData = GetBankCardData();
        cardData.ExpiryYear = expiryYear;

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c.ExpiryYear);
    }

    #endregion

    #region cardExpiration

    [Fact]
    public void Should_have_error_when_card_year_less_than_current_year()
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = "01";
        cardData.ExpiryYear = "20";
        var now = new DateTime(2021, 2, 1);
        _dateTimeProviderMock.Setup(p => p.Now()).Returns(now);

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c)
            .WithErrorMessage(ValidationMessages.BankCard.CardExpired)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_have_error_when_card_year_equal_current_year_and_card_month_less_than_current_month()
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = "01";
        cardData.ExpiryYear = "21";
        var now = new DateTime(2021, 2, 1);
        _dateTimeProviderMock.Setup(p => p.Now()).Returns(now);

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c)
            .WithErrorMessage(ValidationMessages.BankCard.CardExpired)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_not_have_error_when_card_year_greater_than_current_year()
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = "01";
        cardData.ExpiryYear = "22";
        var now = new DateTime(2021, 2, 1);
        _dateTimeProviderMock.Setup(p => p.Now()).Returns(now);

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c);
    }

    [Fact]
    public void Should_not_have_error_when_card_year_equal_current_year_and_card_month_greater_than_current_month()
    {
        var cardData = GetBankCardData();
        cardData.ExpiryMonth = "03";
        cardData.ExpiryYear = "21";
        var now = new DateTime(2021, 2, 1);
        _dateTimeProviderMock.Setup(p => p.Now()).Returns(now);

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c);
    }

    #endregion

    #region cvv

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_cvv_is_null_or_empty(string cvv)
    {
        var cardData = GetBankCardData();
        cardData.Cvv = cvv;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Cvv)
            .WithErrorMessage(ValidationMessages.BankCard.CvvRequired)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("-1")]
    [InlineData("12")]
    [InlineData("12345")]
    [InlineData("a")]
    public void Should_have_error_when_cvv_is_invalid(string cvv)
    {
        var cardData = GetBankCardData();
        cardData.Cvv = cvv;

        var result = _validator.TestValidate(cardData);

        result.ShouldHaveValidationErrorFor(c => c.Cvv)
            .WithErrorMessage(ValidationMessages.BankCard.CvvInvalid)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_not_have_error_when_cvv_is_valid()
    {
        var cardData = GetBankCardData();
        cardData.Cvv = "123";

        var result = _validator.TestValidate(cardData);

        result.ShouldNotHaveValidationErrorFor(c => c.Cvv);
    }

    #endregion

    private BankCardDataDto GetBankCardData()
    {
        return new BankCardDataDto
        {
            Number = "5555555555554444",
            Holder = "Holder",
            ExpiryMonth = "12",
            ExpiryYear = "24",
            Cvv = "519"
        };
    }
}