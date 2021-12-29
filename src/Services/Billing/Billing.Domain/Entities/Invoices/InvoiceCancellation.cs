namespace PaymentGateway.Billing.Domain.Entities.Invoices;

public class InvoiceCancellation : ValueObject
{
    public string Code { get; set; }
    public string Reason { get; set; }
}