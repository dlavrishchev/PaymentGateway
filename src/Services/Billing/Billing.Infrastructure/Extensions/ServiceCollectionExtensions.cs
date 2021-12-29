using Billing.Infrastructure.Data.MongoDb;
using Billing.Infrastructure.Data.MongoDb.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Billing.Domain;
using PaymentGateway.Billing.Domain.Entities.Currencies;
using PaymentGateway.Billing.Domain.Entities.Invoices;
using PaymentGateway.Billing.Domain.Entities.Payments;

namespace Billing.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(MongoDbOptions.SectionName).Get<MongoDbOptions>();
        MongoDbRegistrar.Register();

        var mongoClient = MongoDbClientFactory.Create(options);
        var database = mongoClient.GetDatabase(options.Database);
        services.AddSingleton(_ => database);

        AddRepositories(services);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(MongoDbRepository<>));
        services.AddScoped<IPaymentMethodRepository, PaymentMethodRepository>();
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
    }
}