namespace PaymentGateway.Billing.Application;

public static class ValidationMessages
{
    public static class Invoice
    {
        public const string GuidRequired = "invoice identifier in request is required";
        public const string GuidInvalid = "invoice identifier in request is invalid";
        public const string AmountInvalid = "invalid amount";
        public const string CurrencyRequired = "currency is required";
        public const string CurrencyInvalid = "currency is invalid";
        public const string DescriptionRequired = "invoice description is required";
        public const string DescriptionTooLong = "invoice description too long";
    }

    public static class Shop
    {
        public const string IdRequired = "shop identifier is required";
        public const string IdInvalid = "shop identifier is invalid";
    }

    public static class PaymentMethod
    {
        public const string Required = "payment method identifier in request is required";
        public const string Invalid = "payment method identifier in request is invalid";
    }

    public static class BankCard
    {
        public const string NumberRequired = "card number is required";
        public const string NumberFormatInvalid = "card number format is invalid";
        public const string NumberInvalid = "card number is invalid";

        public const string HolderRequired = "card holder name is required";
        public const string HolderTooLong = "card holder name is too long";

        public const string ExpiryMonthRequired = "card expiration month is required";
        public const string ExpiryMonthInvalid = "card expiration month is invalid";

        public const string ExpiryYearRequired = "card expiration year is required";
        public const string ExpiryYearInvalid = "card expiration year is invalid";

        public const string CardExpired = "card expired";

        public const string CvvRequired = "card CVV is required";
        public const string CvvInvalid = "card CVV is invalid";
    }

}