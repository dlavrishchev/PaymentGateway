using MediatR;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Billing.Application.Queries;

public record GetPaymentFormDataQuery(string InvoiceGuid) : IRequest<PaymentFormData>;