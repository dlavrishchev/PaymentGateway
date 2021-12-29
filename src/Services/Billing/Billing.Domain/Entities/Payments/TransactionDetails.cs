namespace PaymentGateway.Billing.Domain.Entities.Payments;

public class TransactionDetails : ValueObject
{
    public string Rrn { get; set; }
    public string TransactionId { get; set; }
}