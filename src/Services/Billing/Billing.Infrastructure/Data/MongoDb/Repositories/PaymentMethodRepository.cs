using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Payments;

namespace Billing.Infrastructure.Data.MongoDb.Repositories;

public class PaymentMethodRepository : MongoDbRepository<PaymentMethod>, IPaymentMethodRepository
{
    public PaymentMethodRepository(IMongoDatabase database) : base(database)
    {
    }

    public async Task<IList<PaymentMethod>> GetApplicableForInvoiceAsync(Invoice invoice)
    {
        var invoiceCurrency = invoice.Money.CurrencyCode;
        var invoiceAmount = invoice.Money.Amount;

        var filterBuilder = Builders<PaymentMethod>.Filter;
        var filter = filterBuilder.Eq(pm => pm.IsActive, true) &
                     filterBuilder.Eq(pm => pm.Currency, invoiceCurrency) &
                     filterBuilder.Lt(pm => pm.MinPaymentAmount, invoiceAmount) &
                     filterBuilder.Gt(pm => pm.MaxPaymentAmount, invoiceAmount);
        return await collection.Find(filter).ToListAsync();
    }
}