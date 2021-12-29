using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Billing.Domain.Entities.Invoices;

public interface IInvoiceRepository : IRepository<Invoice>
{
    Task<Invoice> GetByGuidAsync(Guid guid, CancellationToken cancellationToken);
}