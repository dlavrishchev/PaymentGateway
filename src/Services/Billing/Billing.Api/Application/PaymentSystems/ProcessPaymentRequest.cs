namespace PaymentGateway.Billing.Application.PaymentSystems;

public record ProcessPaymentRequest(ProcessPaymentRequest.BankCardData CardData, decimal Amount)
{
    public record BankCardData(string Holder, string Number, string ExpiryMonth, string ExpiryYear, string Cvv);
}