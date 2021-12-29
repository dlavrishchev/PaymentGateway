using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.Billing.Application;
using PaymentGateway.Billing.Application.Commands.Invoices;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Shops;
using Xunit;

namespace Billing.UnitTests.Application.Handlers;

public class CreateInvoiceCommandHandlerTests
{
    private readonly Mock<IRepository<Invoice>> _invoiceRepositoryMock;
    private readonly Mock<IRepository<Shop>> _shopRepositoryMock;
    private readonly Mock<ICurrencyRepository> _currencyRepositoryMock;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateInvoiceCommandHandlerTests()
    {
        _invoiceRepositoryMock = new Mock<IRepository<Invoice>>();
        _shopRepositoryMock = new Mock<IRepository<Shop>>();
        _currencyRepositoryMock = new Mock<ICurrencyRepository>();
        _dateTimeProvider = new DefaultDateTimeProvider();
    }

    [Fact]
    public async Task Handle_ShouldReturnCreatedInvoice()
    {
        var shopId = "shop1";
        var currency = "USD";
        var command = new CreateInvoiceCommand(shopId, 1000, currency, "invoiceDescription");
        var shop = new Shop("merchant1", "shopName", "description", "www.shop.example.com", DateTime.UtcNow)
        {
            Id = shopId
        };

        _shopRepositoryMock.Setup(r => r.GetByIdAsync(shopId, default))
            .ReturnsAsync(shop);

        _currencyRepositoryMock.Setup(r => r.IsCurrencyExistAsync(currency, default))
            .ReturnsAsync(true);

        var loggerMock = new Mock<ILogger<CreateInvoiceCommandHandler>>();
        var handler = new CreateInvoiceCommandHandler(_invoiceRepositoryMock.Object, _currencyRepositoryMock.Object,
            _shopRepositoryMock.Object, loggerMock.Object, _dateTimeProvider);

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.IsType<PaymentGateway.Grpc.Billing.Invoice>(result);
        _invoiceRepositoryMock.Verify(c => c.InsertAsync(It.IsAny<Invoice>(), CancellationToken.None), Times.Once);
    }
}