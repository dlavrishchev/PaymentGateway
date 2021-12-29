
namespace PaymentGateway.Billing.Domain.Entities.Merchants;

public class MerchantContact : ValueObject
{
    public MerchantContactAuthorized Authorized { get; private set; }
    public MerchantContactTechnical Technical { get; private set; }

    public MerchantContact(MerchantContactAuthorized authorized, MerchantContactTechnical technical)
    {
        Authorized = authorized;
        Technical = technical;
    }
}