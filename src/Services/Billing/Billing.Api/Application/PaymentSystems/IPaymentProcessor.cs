using System.Threading.Tasks;

namespace PaymentGateway.Billing.Application.PaymentSystems;

public interface IPaymentProcessor
{
    Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request);
}