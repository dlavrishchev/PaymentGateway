namespace PaymentGateway.Billing.Application.Extensions;

static class StringExtensions
{
    public static int? ToNullableInt(this string value)
    {
        return int.TryParse(value, out var result) ? result : null;
    }
}