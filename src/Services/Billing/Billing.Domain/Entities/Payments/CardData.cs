namespace PaymentGateway.Billing.Domain.Entities.Payments;

public class CardData : ValueObject
{
    public string First6 { get; set; }
    public string Last4 { get; set; }
    public string MaskedNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
}