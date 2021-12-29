using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Billing.Application.Exceptions;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Payments;
using PaymentGateway.Billing.Domain.Entities.Shops;
using PaymentGateway.Billing.Domain.Exceptions;
using PaymentGateway.Grpc.Billing;
using Invoice = PaymentGateway.Billing.Domain.Entities.Invoices.Invoice;

namespace PaymentGateway.Billing.Application.Queries;

[UsedImplicitly]
internal class GetPaymentFormDataQueryHandler : IRequestHandler<GetPaymentFormDataQuery, PaymentFormData>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRepository<Shop> _shopRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly ILogger<GetPaymentFormDataQueryHandler> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public GetPaymentFormDataQueryHandler(IInvoiceRepository invoiceRepository,
                                          IRepository<Shop> shopRepository,
                                          ILogger<GetPaymentFormDataQueryHandler> logger, 
                                          IPaymentMethodRepository paymentMethodRepository,
                                          IDateTimeProvider dateTimeProvider)
    {
        _invoiceRepository = invoiceRepository;
        _shopRepository = shopRepository;
        _logger = logger;
        _paymentMethodRepository = paymentMethodRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<PaymentFormData> Handle(GetPaymentFormDataQuery query, CancellationToken cancellationToken)
    {
        var invoice = await GetInvoiceAsync(query.InvoiceGuid, cancellationToken);
        EnsureThatInvoiceCanBePaid(invoice);

        var paymentMethods = await _paymentMethodRepository.GetApplicableForInvoiceAsync(invoice);
        if(paymentMethods.Count == 0)
            throw new ValidationException(Errors.Invoice.ApplicablePaymentMethodsNotFound);

        var shop = await GetShopAsync(invoice.ShopId, cancellationToken);

        var formData = new PaymentFormData
        {
            Amount = (double)invoice.Money.Amount,
            Currency = invoice.Money.CurrencyCode,
            Description = invoice.Description,
            InvoiceGuid = invoice.Guid.ToString(),
            IsTestTransaction = invoice.IsTestInvoice,
            ShopName = shop.Name
        };
        formData.PaymentMethods.AddRange(paymentMethods.Select(pm => new PaymentFormPaymentMethod{Id = pm.Id,  Name = pm.Name}));
        return formData;
    }

    private void EnsureThatInvoiceCanBePaid(Invoice invoice)
    {
        var canBePaid = invoice.CanBePaid(_dateTimeProvider.Now());
        if(!canBePaid.IsSuccess)
            throw new ValidationException(canBePaid.Error);
    }

    private async Task<Invoice> GetInvoiceAsync(string invoiceGuid, CancellationToken cancellationToken)
    {
        var order = await _invoiceRepository.GetByGuidAsync(Guid.Parse(invoiceGuid), cancellationToken);
        if (order == null)
        {
            _logger.LogWarning("Invoice with guid {InvoiceGuid} not found", invoiceGuid);
            throw new ResourceNotFoundException(Errors.Invoice.NotFound);
        }
        return order;
    }

    private async Task<Shop> GetShopAsync(string shopId, CancellationToken cancellationToken)
    {
        var shop = await _shopRepository.GetByIdAsync(shopId, cancellationToken);
        if (shop == null)
        {
            _logger.LogWarning("Shop with id {ShopId} not found", shopId);
            throw new ValidationException(Errors.Shop.NotFound);
        }
        return shop;
    }
}