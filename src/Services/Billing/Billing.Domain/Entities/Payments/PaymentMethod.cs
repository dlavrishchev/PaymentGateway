using System;

namespace PaymentGateway.Billing.Domain.Entities.Payments;

public class PaymentMethod : Entity
{
    public string Name { get; private set; }
    public decimal MinPaymentAmount { get; private set; }
    public decimal MaxPaymentAmount { get; private set; }
    public PaymentMethodType Type { get; private set; }
    public string Currency { get; private set; }
    public bool IsActive { get; set; }

    public bool IsMaxPaymentAmountConfigured => MaxPaymentAmount > 0;

    public PaymentMethod(string name, string currency, decimal minPaymentAmount, decimal maxPaymentAmount)
    {
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        Guard.NotNullOrWhiteSpace(currency, nameof(currency));

        if(minPaymentAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(minPaymentAmount), "minimum payment amount must be >= 0");

        if(maxPaymentAmount < 0)
            throw new ArgumentOutOfRangeException(nameof(maxPaymentAmount), "maximum payment amount must be >= 0");

        if(minPaymentAmount > maxPaymentAmount)
            throw new ArgumentException("minPaymentAmount must less or equal maxPaymentAmount", nameof(minPaymentAmount));

        Name = name;
        Currency = currency;
        MinPaymentAmount = minPaymentAmount;
        MaxPaymentAmount = maxPaymentAmount;
        Type = PaymentMethodType.BankCard;
    }
}