using System;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Shops;
using Xunit;

namespace Billing.UnitTests.Domain;

public class ShopTests
{
    [Fact]
    public void CreateInvoice_Success()
    {
        var shopId = "6036347d1617680f6fb192d2";
        var shop = new Shop("6036347d1617680f6fb192ce", "shopName", "shopDescription", "shopUrl", DateTime.UtcNow)
        {
            Id = shopId
        };

        var invoice = shop.CreateInvoice(Money.Create("USD",1000), DateTime.MaxValue, "description", DateTime.UtcNow);

        Assert.NotNull(invoice);
        Assert.Equal(shopId, invoice.ShopId);
    }
}