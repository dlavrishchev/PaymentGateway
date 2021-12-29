using System;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Payments;
using Xunit;

namespace Billing.UnitTests.Domain;

public class InvoiceTests
{
    [Fact]
    public void CanBePaid_WhenInvoiceIsNotPaidEarly_ReturnSuccess()
    {
        var invoice = new Invoice("6036347d1617680f6fb192d2", Money.Create("USD", 1000), DateTime.MaxValue, DateTime.UtcNow, "description", true);

        var result = invoice.CanBePaid(DateTime.UtcNow);
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CanBePaid_WhenInvoiceIsExpired_ReturnInvoiceExpiredError()
    {
        var expirationDate = DateTime.UtcNow;
        var invoice = new Invoice("6036347d1617680f6fb192d2", Money.Create("USD", 1000), expirationDate, DateTime.UtcNow, "description", true);
        var expectedError = Errors.Invoice.Expired;

        var result = invoice.CanBePaid(expirationDate.AddMinutes(10));

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void CanBePaid_WhenInvoicePaidEarly_ReturnInvoicePaidEarlyError()
    {
        var invoice = new Invoice("6036347d1617680f6fb192d2", Money.Create("USD", 1000), DateTime.MaxValue, DateTime.UtcNow, "description", true);
        invoice.MakePaid(CreateDefaultPaymentMethod(), CreateDefaulTransactionDetails(), CreateDefaultCardData(), DateTime.UtcNow);
        var expectedError = Errors.Invoice.AlreadyProcessed;
        
        var result = invoice.CanBePaid(DateTime.UtcNow);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedError, result.Error);
    }

    [Fact]
    public void MakePaid_WhenIsNotPaidEarly_Success()
    {
        var invoice = new Invoice("6036347d1617680f6fb192d2", Money.Create("USD", 1000), DateTime.MaxValue, DateTime.UtcNow, "description", true);
        var expectedStatus = InvoiceStatus.Paid;

        invoice.MakePaid(CreateDefaultPaymentMethod(), CreateDefaulTransactionDetails(), CreateDefaultCardData(), DateTime.UtcNow);

        Assert.Equal(expectedStatus, invoice.Status);
    }

    private PaymentMethod CreateDefaultPaymentMethod()
    {
        return new PaymentMethod("pm1", "USD", 100, 100_000){Id = "6036347d1617680f6fb192cc"};
    }

    private TransactionDetails CreateDefaulTransactionDetails()
    {
        return new TransactionDetails { Rrn = "rrn", TransactionId = "tnxId" };
    }

    private CardData CreateDefaultCardData()
    {
        return new CardData { MaskedNumber = "400000******3184", First6 = "400000", Last4 = "3184", ExpiryMonth = 2, ExpiryYear = 24 };
    }
}