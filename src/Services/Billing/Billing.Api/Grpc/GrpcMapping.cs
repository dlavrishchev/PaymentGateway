using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Billing.Grpc;

internal static class GrpcMapping
{
    public static Invoice ToGrpcInvoice(this Domain.Entities.Invoices.Invoice invoice)
    {
        return new Invoice
        {
            Amount = (double)invoice.Money.Amount,
            Currency = invoice.Money.CurrencyCode,
            Description = invoice.Description,
            Guid = invoice.Guid.ToString()
        };
    }
}