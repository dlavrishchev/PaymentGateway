using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Shops;
using PaymentGateway.Billing.Domain.Exceptions;
using PaymentGateway.Billing.Grpc;

namespace PaymentGateway.Billing.Application.Commands.Invoices;

[UsedImplicitly]
public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, PaymentGateway.Grpc.Billing.Invoice>
{
    private readonly IRepository<Invoice> _invoiceRepository;
    private readonly ICurrencyRepository _currencyRepository;
    private readonly IRepository<Shop> _shopRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ILogger<CreateInvoiceCommandHandler> _logger;

    public CreateInvoiceCommandHandler(IRepository<Invoice> invoiceRepository,
                                       ICurrencyRepository currencyRepository, 
                                       IRepository<Shop> shopRepository,
                                       ILogger<CreateInvoiceCommandHandler> logger,
                                       IDateTimeProvider dateTimeProvider)
    {
        _invoiceRepository = invoiceRepository;
        _currencyRepository = currencyRepository;
        _shopRepository = shopRepository;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaymentGateway.Grpc.Billing.Invoice> Handle(CreateInvoiceCommand command, CancellationToken cancellationToken)
    {
        var shop = await GetShopAsync(command.ShopId, cancellationToken);
        EnsureThatShopCanCreateInvoice(shop);

        await EnsureThatCurrencyExistAsync(command.Currency, cancellationToken);

        var invoice = shop.CreateInvoice(
            Money.Create(command.Currency, command.Amount),
            CreateInvoiceExpirationDate(),
            command.Description,
            _dateTimeProvider.Now());

        await _invoiceRepository.InsertAsync(invoice, cancellationToken);

        _logger.LogInformation("Invoice with id {InvoiceId} created", invoice.Id);
        return invoice.ToGrpcInvoice();
    }

    private void EnsureThatShopCanCreateInvoice(Shop shop)
    {
        var result = shop.CanCreateInvoice();
        if(!result.IsSuccess)
            throw new ValidationException(result.Error);
    }

    private async Task EnsureThatCurrencyExistAsync(string currencyCode, CancellationToken cancellationToken)
    {
        if(!await _currencyRepository.IsCurrencyExistAsync(currencyCode, cancellationToken))
            throw new ValidationException(Errors.Currency.NotFound);
    }

    private async Task<Shop> GetShopAsync(string shopId, CancellationToken cancellationToken)
    {
        var shop = await _shopRepository.GetByIdAsync(shopId, cancellationToken);
        if(shop == null)
            throw new ValidationException(Errors.Shop.NotFound);
        return shop;
    }

    private DateTime CreateInvoiceExpirationDate()
    {
        return _dateTimeProvider.Now().AddMinutes(10); //todo: get from system settings.
    }
}