using System;
using PaymentGateway.Billing.Domain.Entities.Shops;

namespace PaymentGateway.Billing.Domain.Entities.Merchants;

public class Merchant : Entity
{
    public MerchantCompany Company { get; private set; }
    public MerchantBank Bank { get; private set; }
    public MerchantContact Contact { get; private set; }
    public MerchantStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Merchant()
    {
        Status = MerchantStatus.Draft;
    }

    public Shop CreateShop(string name, string description, string url, DateTime now)
    {
        return new Shop(Id, name, description, url, now);
    }
}