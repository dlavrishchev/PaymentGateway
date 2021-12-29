using System;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Payments;
using PaymentGateway.Billing.Domain.Exceptions;

namespace PaymentGateway.Billing.Domain.Entities.Invoices;

public class Invoice : Entity
{
    public Guid Guid { get; private set; }
    public string ShopId { get; private set; }
    public InvoiceStatus Status { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? PaidAt { get; private set; }
    public DateTime? DeclinedAt { get; private set; }
    public DateTime ExpirationDate { get; private set; }
    public InvoiceCancellation Cancellation { get; private set; }
    public Money Money { get; private set; }
    public string PaymentMethodId { get; private set; }
    public TransactionDetails TransactionDetails { get; private set; }
    public CardData CardData { get; private set; }
    public bool IsTestInvoice { get; private set; }

    internal Invoice(string shopId,
                     Money money,
                     DateTime expirationDate,
                     DateTime createdAt,
                     string description,
                     bool isTestInvoice)
    {

        Guard.NotNullOrWhiteSpace(shopId, nameof(shopId));
        Guard.NotNull(money, nameof(money));
        Guard.NotNullOrWhiteSpace(description, nameof(description));

        Guid = Guid.NewGuid();
        ShopId = shopId;
        Money = money;
        ExpirationDate = expirationDate;
        Description = description;
        IsTestInvoice = isTestInvoice;
        CreatedAt = createdAt;

        Status = InvoiceStatus.New;
    }

    public Result CanBePaid(DateTime now)
    {
        if(IsHasProcessedStatus())
            return Result.Fail(Errors.Invoice.AlreadyProcessed);

        if(Status != InvoiceStatus.New)
            return Result.Fail(Errors.Invoice.InvalidState);

        if(IsExpired(now))
            return Result.Fail(Errors.Invoice.Expired);

        return Result.Success();
    }

    public Result CanBeDecline(DateTime now)
    {
        return CanBePaid(now);
    }

    public void MakePaid(PaymentMethod paymentMethod, TransactionDetails transactionDetails, CardData cardData, DateTime now)
    {
        Guard.NotNull(paymentMethod, nameof(paymentMethod));
        Guard.NotNull(transactionDetails, nameof(transactionDetails));
        Guard.NotNull(cardData, nameof(cardData));

        var canBePaidResult = CanBePaid(now);
        if(!canBePaidResult.IsSuccess)
            throw new ValidationException(canBePaidResult.Error); 
        //todo: возможно стоит выбрасывать DomainBrokenInvariantException -> http 500

        PaymentMethodId = paymentMethod.Id;
        PaidAt = UpdatedAt = now;
        TransactionDetails = transactionDetails;
        CardData = cardData;
        Status = InvoiceStatus.Paid;
    }

    public void MakeDeclined(TransactionDetails transactionDetails, InvoiceCancellation cancellation, CardData cardData, DateTime now)
    {
        Guard.NotNull(transactionDetails, nameof(transactionDetails));
        Guard.NotNull(cancellation, nameof(cancellation));
        Guard.NotNull(cardData, nameof(cardData));

        var canBeDeclineResult = CanBeDecline(now);
        if(!canBeDeclineResult.IsSuccess)
            throw new ValidationException(canBeDeclineResult.Error);

        Cancellation = cancellation;
        CardData = cardData;
        DeclinedAt = UpdatedAt = now;
        TransactionDetails = transactionDetails;
        Status = InvoiceStatus.Declined;
    }


    public Result IsPaymentMethodCanBeApplied(PaymentMethod paymentMethod)
    {
        if (!IsInvoiceCurrencyMatchPaymentMethodCurrency(paymentMethod))
            return Result.Fail(Errors.Invoice.CurrencyMismatch);

        if(!IsInvoiceAmountGreatThanMinAllowed(paymentMethod))
            return Result.Fail(Errors.Invoice.AmountLessThanAllowed);

        if(!IsInvoiceAmountLessThanMaxAllowed(paymentMethod))
            return Result.Fail(Errors.Invoice.AmountGreatThanAllowed);

        return Result.Success();
    }

    private bool IsExpired(DateTime now)
    {
        if (Status is InvoiceStatus.Expired)
            return true;

        if (now > ExpirationDate)
            return true;

        return false;
    }

    private bool IsInvoiceCurrencyMatchPaymentMethodCurrency(PaymentMethod paymentMethod)
    {
        return string.Equals(paymentMethod.Currency, Money.CurrencyCode, StringComparison.Ordinal);
    }

    private bool IsInvoiceAmountGreatThanMinAllowed(PaymentMethod paymentMethod)
    {
        return Money.Amount > paymentMethod.MinPaymentAmount;
    }

    private bool IsInvoiceAmountLessThanMaxAllowed(PaymentMethod paymentMethod)
    {
        var orderAmountLessThanMaxAllowed = true;
        if (paymentMethod.IsMaxPaymentAmountConfigured)
            orderAmountLessThanMaxAllowed = Money.Amount < paymentMethod.MaxPaymentAmount;
        return orderAmountLessThanMaxAllowed;
    }

    private bool IsHasProcessedStatus()
    {
        if (Status is InvoiceStatus.Paid or InvoiceStatus.Declined or InvoiceStatus.Canceled)
            return true;
        return false;
    }
}