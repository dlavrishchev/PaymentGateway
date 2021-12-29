using System;

namespace PaymentGateway.Billing.Domain.Entities.Currencies;

public class Money : ValueObject
{
    public string CurrencyCode { get; private set; }
    public decimal Amount { get; private set; }

    private Money(string currency, decimal amount)
    {
        CurrencyCode = currency;
        Amount = amount;
    }

    public static Money Create(string currency, decimal amount)
    {
        Guard.NotNullOrWhiteSpace(currency, nameof(currency));

        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be positive value.");

        return new Money(currency, amount);
    }
}