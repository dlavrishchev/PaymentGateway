namespace PaymentGateway.Billing.Domain.Entities.Merchants;

public class MerchantContactAuthorized : ValueObject
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Position { get; set; }
}