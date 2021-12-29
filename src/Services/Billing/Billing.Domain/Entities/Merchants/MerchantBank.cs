namespace PaymentGateway.Billing.Domain.Entities.Merchants;

public class MerchantBank : ValueObject
{
    public string Name { get; set; }
    public string AccountNumber { get; set; }
    public string CorrespondentAccount { get; set; }
    public string Bik { get; set; }
}