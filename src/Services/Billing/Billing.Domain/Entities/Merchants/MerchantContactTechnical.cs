namespace PaymentGateway.Billing.Domain.Entities.Merchants;

public class MerchantContactTechnical : ValueObject
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}