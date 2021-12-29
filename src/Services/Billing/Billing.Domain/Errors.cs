namespace PaymentGateway.Billing.Domain;

public static class Errors
{
    public static Error InternalError => new("B0000", "internal error");
    public static Error InvalidRequest => new("B4000", "invalid request");

    public static class Invoice
    {
        public static Error AlreadyProcessed => new("B0001", "invoice already processed");
        public static Error InvalidState => new("B0002", "invalid invoice state");
        public static Error Expired => new("B0003", "invoice expired");
        public static Error NotFound => new("B0004", "invoice with specified identifier not found");
        public static Error CurrencyMismatch => new("B0005", "invoice currency not match payment method currency");
        public static Error AmountLessThanAllowed => new("B0006", "invoice amount less than minimum allowed");
        public static Error AmountGreatThanAllowed => new("B0007", "invoice amount great than maximum allowed");
        public static Error ApplicablePaymentMethodsNotFound => new("B0008", "applicable payment methods not found");
    }

    public static class Shop
    {
        public static Error NotFound => new("B1001", "shop not found");
        public static Error Inactive => new("B1002", "shop inactive");
    }

    public static class Currency
    {
        public static Error InvalidCurrencyCode => new("B2001", "invalid currency code");
        public static Error NotFound => new("B2002", "currency not found");
    }

    public static class PaymentMethod
    {
        public static Error Inactive => new("B3001", "payment method inactive");
        public static Error NotFound => new("B3002", "payment method not found");
    }
}

public record Error(string ErrorCode, string Message)
{
    public override string ToString()
    {
        return $"{ErrorCode}:{Message}";
    }
}