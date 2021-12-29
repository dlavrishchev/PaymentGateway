using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using PaymentGateway.Billing.Domain.Entities.Currencies;

namespace Billing.Infrastructure.Data.MongoDb.Repositories;

public class CurrencyRepository : MongoDbRepository<Currency>, ICurrencyRepository
{
    public CurrencyRepository(IMongoDatabase database) : base(database)
    {
    }
        
    public async Task<Currency> GetCurrencyByCodeAsync(string currencyCode, CancellationToken cancellationToken)
    {
        return await FindAsync(c => c.Code == currencyCode, cancellationToken);
    }

    public async Task<bool> IsCurrencyExistAsync(string currencyCode, CancellationToken cancellationToken)
    {
        var currency = await GetCurrencyByCodeAsync(currencyCode, cancellationToken);
        return currency != null;
    }
}