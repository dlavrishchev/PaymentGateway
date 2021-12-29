namespace PaymentGateway.Billing.Domain.Entities.Invoices;

public enum InvoiceStatus
{
    New = 0,
    Paid = 1,
    Declined = 2,
    Expired = 3,
    Canceled = 4
}