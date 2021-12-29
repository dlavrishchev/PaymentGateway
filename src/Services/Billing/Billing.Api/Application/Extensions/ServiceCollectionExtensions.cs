using System;
using Billing.Infrastructure.Data.MongoDb;
using Billing.Infrastructure.Extensions;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PaymentGateway.Billing.Application.Behaviors;
using PaymentGateway.Billing.Application.PaymentSystems;

namespace PaymentGateway.Billing.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBillingServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentProcessor, SandboxPaymentProcessor>();
        services.AddTransient<IBankCardNumberMasker, BankCardNumberMasker>();
        services.AddTransient<IDateTimeProvider, DefaultDateTimeProvider>();
        AddMediator(services);
        AddFluentValidation(services);
        return services;
    }

    public static IServiceCollection AddBillingHealthChecks(this IServiceCollection services)
    {
        var hcBuilder = services.AddHealthChecks();
        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
        hcBuilder.AddMongoDb(
            name: "mongodb-check",
            tags: new[] { "mongodb" },
            timeout: TimeSpan.FromSeconds(5));

        return services;
    }

    private static void AddFluentValidation(IServiceCollection services)
    {
        services.AddFluentValidation();
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        ValidatorOptions.Global.CascadeMode = CascadeMode.Stop; //Important!
    }

    private static void AddMediator(IServiceCollection services)
    {
        services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}