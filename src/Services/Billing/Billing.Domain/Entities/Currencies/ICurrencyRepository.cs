using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Billing.Domain.Entities.Currencies;

public interface ICurrencyRepository : IRepository<Currency>
{
    Task<Currency> GetCurrencyByCodeAsync(string currencyCode, CancellationToken cancellationToken);
    Task<bool> IsCurrencyExistAsync(string currencyCode, CancellationToken cancellationToken);
}