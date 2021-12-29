using System.Threading.Tasks;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Checkout.Services;

public interface IBillingService
{
    Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request);
    Task<PaymentFormData> GetPaymentFormDataAsync(GetPaymentFormDataRequest request);
    Task<ProcessPaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request);
}