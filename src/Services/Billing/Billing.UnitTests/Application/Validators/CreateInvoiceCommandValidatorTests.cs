using FluentValidation.TestHelper;
using PaymentGateway.Billing.Application;
using PaymentGateway.Billing.Application.Commands.Invoices;
using PaymentGateway.Billing.Domain;
using Xunit;

namespace Billing.UnitTests.Application.Validators;

public class CreateInvoiceCommandValidatorTests
{
    private readonly CreateInvoiceCommandValidator _validator;
    private readonly string _errorCode;

    public CreateInvoiceCommandValidatorTests()
    {
        _validator = new CreateInvoiceCommandValidator();
        _errorCode = Errors.InvalidRequest.ErrorCode;
    }

    #region shopId

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_have_error_when_shopId_is_null_or_empty(string shopId)
    {
        var command = new CreateInvoiceCommand(shopId, 1000, "USD", "description");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.ShopId)
            .WithErrorMessage(ValidationMessages.Shop.IdRequired)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("abcd")]
    [InlineData("6036347d1617680f6fb192Y2")]
    [InlineData(".!<>,-")]
    public void Should_have_error_when_shopId_is_invalid(string shopId)
    {
        var command = new CreateInvoiceCommand(shopId, 1000, "USD", "description");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.ShopId)
            .WithErrorMessage(ValidationMessages.Shop.IdInvalid)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("abcdefabcdefabcdefabcdef")]
    [InlineData("ABCDEFABCDEFABCDEFABCDEF")]
    [InlineData("603634741617680561819272")]
    [InlineData("6036347d1617680f6fb192a2")]
    [InlineData("6036347d1617680F6fb192A2")]
    public void Should_not_have_error_when_shopId_is_valid(string shopId)
    {
        var command = new CreateInvoiceCommand(shopId, 1000, "USD", "description");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.ShopId);
    }

    #endregion

    #region amount

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Should_have_error_when_amount_is_invalid(decimal amount)
    {
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", amount, "USD", "description");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Amount)
            .WithErrorMessage(ValidationMessages.Invoice.AmountInvalid)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_not_have_error_when_amount_is_valid()
    {
        var amount = 1000;
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", amount, "USD", "description");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Amount);
    }

    #endregion

    #region currency

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_have_error_when_currency_is_null_or_empty(string currency)
    {
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", 1000, currency, "description");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Currency)
            .WithErrorMessage(ValidationMessages.Invoice.CurrencyRequired)
            .WithErrorCode(_errorCode);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("a")]
    [InlineData("R UB")]
    [InlineData("RUB ")]
    [InlineData(" RUB")]
    [InlineData("rub")]
    public void Should_have_error_when_currency_is_invalid(string currency)
    {
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", 1000, currency, "description");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Currency)
            .WithErrorMessage(ValidationMessages.Invoice.CurrencyInvalid)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_not_have_error_when_currency_is_valid()
    {
        var currency = "RUB";
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", 1000, currency, "description");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Currency);
    }

    #endregion

    #region description

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_have_error_when_description_is_null_or_empty(string description)
    {
        var errorCode = Errors.InvalidRequest.ErrorCode;
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", 1000, "USD", description);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.Invoice.DescriptionRequired)
            .WithErrorCode(errorCode);
    }

    [Fact]
    public void Should_have_error_when_description_is_too_long()
    {
        var description = new string('a', 1001);
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", 1000, "USD", description);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Description)
            .WithErrorMessage(ValidationMessages.Invoice.DescriptionTooLong)
            .WithErrorCode(_errorCode);
    }

    [Fact]
    public void Should_not_have_error_when_description_is_valid()
    {
        var description = new string('a', 1000);
        var command = new CreateInvoiceCommand("6036347d1617680f6fb192a2", 1000, "USD", description);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Description);
    }

    #endregion
}