using MediatR;
using PaymentGateway.Billing.Application.Dto;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Billing.Application.Commands.Payments;

public record ProcessPaymentCommand(string InvoiceGuid, string PaymentMethodId, BankCardDataDto BankCardData) : IRequest<ProcessPaymentResponse>;