using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentGateway.Billing.Domain.Entities.Invoices;

namespace Billing.Infrastructure.Data.MongoDb.Repositories;

public class InvoiceRepository : MongoDbRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(IMongoDatabase database) : base(database)
    {
    }

    public async Task<Invoice> GetByGuidAsync(Guid guid, CancellationToken cancellationToken)
    {
        return await FindAsync(e => e.Guid == guid, cancellationToken);
    }
}