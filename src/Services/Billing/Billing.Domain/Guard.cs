using System;

namespace PaymentGateway.Billing.Domain;

public static class Guard
{
    public static void NotNullOrWhiteSpace(string value, string parameterName)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} can not be null, empty or white space.", parameterName);
    }

    public static void NotNull<T>(T value, string parameterName)
    {
        if(value == null)
            throw new ArgumentNullException(parameterName);
    }
}