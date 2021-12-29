using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentGateway.Billing.Domain.Entities.Invoices;

namespace PaymentGateway.Billing.Domain.Entities.Payments;

public interface IPaymentMethodRepository : IRepository<PaymentMethod>
{
    Task<IList<PaymentMethod>> GetApplicableForInvoiceAsync(Invoice invoice);
}