using System;

namespace PaymentGateway.Billing.Application.Extensions;

public static class DateTimeExtensions
{
    public static int YearLastTwoDigits(this DateTime dt) => dt.Year % 100;
}