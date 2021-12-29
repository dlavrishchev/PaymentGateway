using MediatR;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Billing.Application.Commands.Invoices;

public record CreateInvoiceCommand(string ShopId, decimal Amount, string Currency, string Description) : IRequest<Invoice>;