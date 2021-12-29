using System;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Exceptions;

namespace PaymentGateway.Billing.Domain.Entities.Shops;

public class Shop : Entity
{
    public string MerchantId { get; private set; }
    public string Name { get; private set;}
    public string Description { get; private set; }
    public string Url { get; set; }
    public ShopWebhookNotification WebhookNotification { get; private set; }
    public ShopRedirect RedirectDetails { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public ShopStatus Status { get; private set; }

    public bool IsActive => Status != ShopStatus.Deleted && Status != ShopStatus.Suspended;

    internal Shop(string merchantId, string name, string description, string url, DateTime createdAt)
    {
        Guard.NotNull(merchantId, nameof(merchantId));
        Guard.NotNullOrWhiteSpace(name, nameof(name));
        Guard.NotNullOrWhiteSpace(url, nameof(url));
        Guard.NotNullOrWhiteSpace(description, nameof(description));

        MerchantId = merchantId;
        Name = name;
        Url = url;
        Status = ShopStatus.Draft;
        CreatedAt = createdAt;
    }

    public Invoice CreateInvoice(Money money, DateTime expirationDate, string description, DateTime now)
    {
        var result = CanCreateInvoice();
        if(!result.IsSuccess)
            throw new ValidationException(result.Error);

        var isTestInvoice = Status == ShopStatus.Draft;
        return new Invoice(Id, money, expirationDate, now, description, isTestInvoice);
    }

    public Result CanCreateInvoice()
    {
        return IsActive ? Result.Success() : Result.Fail(Errors.Shop.Inactive);
    }
}