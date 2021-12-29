using System;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentGateway.Checkout.Configuration;
using PaymentGateway.Checkout.Services;

namespace PaymentGateway.Checkout.Extensions;

public static class GrpcServiceCollectionExtensions
{
    public static IServiceCollection AddGrpcClients(this IServiceCollection services)
    {
        AddGrpcClient<PaymentGateway.Grpc.Billing.BillingService.BillingServiceClient>(services, BillingService.Name);
        return services;
    }

    private static void AddGrpcClient<TClient>(IServiceCollection services, string serviceName) where TClient:class
    {
        var builder = services.AddGrpcClient<TClient>((serviceProvider, options) =>
        {
            var settingsAccessor = serviceProvider.GetRequiredService<IOptionsMonitor<RpcServiceSettings>>();
            var settings = settingsAccessor.Get(serviceName);
            options.Address = new Uri(settings.Url);
        });

        builder.ConfigureChannel((serviceProvider, options) =>
        {
            var settingsAccessor = serviceProvider.GetRequiredService<IOptionsMonitor<RpcServiceSettings>>();
            var settings = settingsAccessor.Get(serviceName);

            options.ServiceConfig = new ServiceConfig
            {
                MethodConfigs = {CreateDefaultMethodConfig(settings.Retry.Count)}
            };
        });
    }

    private static MethodConfig CreateDefaultMethodConfig(int retryCount)
    {
        return new MethodConfig
        {
            Names = { MethodName.Default }, // applicable for all service methods.
            RetryPolicy = new RetryPolicy
            {
                MaxAttempts = retryCount,
                InitialBackoff = TimeSpan.FromSeconds(1),
                MaxBackoff = TimeSpan.FromSeconds(3),
                BackoffMultiplier = 1,
                RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.DeadlineExceeded }
            }
        };
    }
}