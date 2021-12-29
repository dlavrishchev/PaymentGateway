using System;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using PaymentGateway.Billing.Application.Commands.Invoices;
using PaymentGateway.Billing.Application.Commands.Payments;
using PaymentGateway.Billing.Application.Dto;
using PaymentGateway.Billing.Application.Queries;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Billing.Grpc;

public class BillingService : PaymentGateway.Grpc.Billing.BillingService.BillingServiceBase
{
    private readonly IMediator _mediator;
    public BillingService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<Invoice> CreateInvoice(CreateInvoiceRequest request, ServerCallContext context)
    {
        var command = new CreateInvoiceCommand(
            request.ShopId,
            Convert.ToDecimal(request.Amount),
            request.Currency,
            request.Description);

        return await _mediator.Send(command);
    }

    public override async Task<PaymentFormData> GetPaymentFormData(GetPaymentFormDataRequest request, ServerCallContext context)
    {
        var query = new GetPaymentFormDataQuery(request.InvoiceGuid);
        return await _mediator.Send(query);
    }

    public override async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request, ServerCallContext context)
    {
        var cardDataDto = new BankCardDataDto
        {
            Holder = request.BankCardRequisites.Holder,
            Number = request.BankCardRequisites.Number,
            ExpiryMonth = request.BankCardRequisites.ExpiryMonth,
            ExpiryYear = request.BankCardRequisites.ExpiryYear,
            Cvv = request.BankCardRequisites.Cvv
        };

        var command = new ProcessPaymentCommand(request.InvoiceGuid, request.PaymentMethodId, cardDataDto);
        return await _mediator.Send(command);
    }
}