using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using PaymentGateway.Billing.Application.Dto;
using PaymentGateway.Billing.Application.PaymentSystems;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Payments;
using PaymentGateway.Billing.Domain.Entities.Shops;
using PaymentGateway.Billing.Domain.Exceptions;
using PaymentGateway.Grpc.Billing;
using Invoice = PaymentGateway.Billing.Domain.Entities.Invoices.Invoice;
using ProcessPaymentRequest = PaymentGateway.Billing.Application.PaymentSystems.ProcessPaymentRequest;

namespace PaymentGateway.Billing.Application.Commands.Payments;

[UsedImplicitly]
internal class ProcessPaymentCommandHandler: IRequestHandler<ProcessPaymentCommand, ProcessPaymentResponse>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IRepository<Shop> _shopRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IPaymentProcessor _paymentProcessor;
    private readonly IBankCardNumberMasker _bankCardNumberMasker;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProcessPaymentCommandHandler(IInvoiceRepository invoiceRepository,
                                        IRepository<Shop> shopRepository,
                                        IPaymentMethodRepository paymentMethodRepository,
                                        IPaymentProcessor paymentProcessor,
                                        IBankCardNumberMasker bankCardNumberMasker,
                                        IDateTimeProvider dateTimeProvider)
    {
        _invoiceRepository = invoiceRepository;
        _shopRepository = shopRepository;
        _paymentMethodRepository = paymentMethodRepository;
        _paymentProcessor = paymentProcessor;
        _bankCardNumberMasker = bankCardNumberMasker;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ProcessPaymentResponse> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        var invoice = await GetInvoiceAsync(Guid.Parse(command.InvoiceGuid), cancellationToken);
        EnsureThatInvoiceCanBePaid(invoice);

        var shop = await GetShopAsync(invoice.ShopId, cancellationToken);
        EnsureThatShopIsActive(shop);

        var paymentMethod = await GetPaymentMethodAsync(command.PaymentMethodId, cancellationToken);
        EnsureThatPaymentMethodIsActive(paymentMethod);
        EnsureThatPaymentMethodCanBeApplied(paymentMethod, invoice);

        var paymentRequest = CreatePaymentRequest(command.BankCardData, invoice.Money.Amount);
        var paymentResult = await _paymentProcessor.ProcessPaymentAsync(paymentRequest);
        var transactionDetails = CreateTransactionDetails(paymentResult);
        var invoiceCardData = CreateInvoiceCardData(command.BankCardData);

        if (paymentResult.IsSuccess)
        {
            invoice.MakePaid(paymentMethod, transactionDetails, invoiceCardData, _dateTimeProvider.Now());
        }
        else
        {
            var cancellation = CreateInvoiceCancellation(paymentResult);
            invoice.MakeDeclined(transactionDetails, cancellation, invoiceCardData, _dateTimeProvider.Now());
        }

        await _invoiceRepository.UpdateAsync(invoice, cancellationToken);
        return CreateResponse(paymentResult, shop.RedirectDetails, invoiceCardData.MaskedNumber);
    }

    private void EnsureThatInvoiceCanBePaid(Invoice invoice)
    {
        var result = invoice.CanBePaid(_dateTimeProvider.Now());
        if(!result.IsSuccess)
            throw new ValidationException(result.Error);
    }

    private void EnsureThatShopIsActive(Shop shop)
    {
        if(!shop.IsActive)
            throw new ValidationException(Errors.Shop.Inactive);
    }

    private void EnsureThatPaymentMethodIsActive(PaymentMethod paymentMethod)
    {
        if(!paymentMethod.IsActive)
            throw new ValidationException(Errors.PaymentMethod.Inactive);
    }

    private void EnsureThatPaymentMethodCanBeApplied(PaymentMethod paymentMethod, Invoice invoice)
    {
        var result = invoice.IsPaymentMethodCanBeApplied(paymentMethod);
        if(!result.IsSuccess)
            throw new ValidationException(result.Error);
    }

    private ProcessPaymentRequest CreatePaymentRequest(BankCardDataDto cardData, decimal amount)
    {
        return new ProcessPaymentRequest(
            new ProcessPaymentRequest.BankCardData(
                cardData.Holder,
                cardData.Number,
                cardData.ExpiryMonth,
                cardData.ExpiryYear,
                cardData.Cvv), amount);
    }

    private CardData CreateInvoiceCardData(BankCardDataDto cardData)
    {
        return new CardData
        {
            First6 = cardData.Number[..5],
            Last4 = cardData.Number[^4..],
            MaskedNumber = _bankCardNumberMasker.Mask(cardData.Number),
            ExpiryMonth = int.Parse(cardData.ExpiryMonth),
            ExpiryYear = int.Parse(cardData.ExpiryYear)
        };
    }

    private TransactionDetails CreateTransactionDetails(ProcessPaymentResult paymentResult)
    {
        return new TransactionDetails
        {
            TransactionId = paymentResult.TransactionId,
            Rrn = paymentResult.Rrn
        };
    }

    private InvoiceCancellation CreateInvoiceCancellation(ProcessPaymentResult paymentResult)
    {
        return new InvoiceCancellation
        {
            Code = paymentResult.Decline.Code,
            Reason = paymentResult.Decline.Reason
        };
    }

    private ProcessPaymentResponse CreateResponse(ProcessPaymentResult paymentResult, ShopRedirect shopRedirect, string maskedCardNumber)
    {
        var response = new ProcessPaymentResponse
        {
            IsSuccess = paymentResult.IsSuccess,
            SuccessUrl = shopRedirect.SuccessUrl, 
            FailUrl = shopRedirect.FailUrl,
            TransactionId = paymentResult.TransactionId,
            MaskedCardNumber = maskedCardNumber
        };

        if (paymentResult.IsSuccess)
        {
            response.Rrn = paymentResult.Rrn;
        }
        else
        {
            response.DeclineCode = paymentResult.Decline.Code;
            response.DeclineReason = paymentResult.Decline.Reason;
        }

        return response;
    }

    private async Task<Invoice> GetInvoiceAsync(Guid invoiceGuid, CancellationToken cancellationToken)
    {
        var invoice = await _invoiceRepository.GetByGuidAsync(invoiceGuid, cancellationToken);
        if(invoice == null)
            throw new ValidationException(Errors.Invoice.NotFound);

        return invoice;
    }

    private async Task<Shop> GetShopAsync(string shopId, CancellationToken cancellationToken)
    {
        var shop = await _shopRepository.GetByIdAsync(shopId, cancellationToken);
        if(shop == null)
            throw new ValidationException(Errors.Shop.NotFound);

        return shop;
    }

    private async Task<PaymentMethod> GetPaymentMethodAsync(string paymentMethodId, CancellationToken cancellationToken)
    {
        var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodId, cancellationToken);
        if (paymentMethod == null)
            throw new ValidationException(Errors.PaymentMethod.NotFound);

        return paymentMethod;
    }
}