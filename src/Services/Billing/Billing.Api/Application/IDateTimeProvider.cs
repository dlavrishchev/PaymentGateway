using System;

namespace PaymentGateway.Billing.Application;

public interface IDateTimeProvider
{
    public DateTime Now();
}

public class DefaultDateTimeProvider : IDateTimeProvider
{
    public DateTime Now()
    {
        return DateTime.UtcNow;
    }
}