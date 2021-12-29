namespace PaymentGateway.Billing.Application.Dto;

public class BankCardDataDto
{
    public string Number { get; set; }
    public string Holder { get; set; }
    public string Cvv { get; set; }
    public string ExpiryMonth { get; set; }
    public string ExpiryYear { get; set; } // two-digit format
}