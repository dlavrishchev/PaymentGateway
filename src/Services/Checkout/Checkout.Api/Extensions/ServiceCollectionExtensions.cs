using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using PaymentGateway.Checkout.Configuration;
using PaymentGateway.Checkout.Factories;
using PaymentGateway.Checkout.Infrastructure;
using PaymentGateway.Checkout.Services;

namespace PaymentGateway.Checkout.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRpcServiceSettings(configuration, BillingService.Name);
        return services;
    }

    public static IServiceCollection AddBaseServices(this IServiceCollection services)
    {
        services.AddScoped<IBillingService, BillingService>();
        services.AddScoped<PaymentViewModelDropDownItemsProvider>();
        services.AddScoped<PaymentViewModelFactory>();
        services.AddScoped<IExceptionToApiErrorConverter, DefaultExceptionToApiErrorConverter>();
        services.AddScoped<IHttpExceptionStatusCodeProvider, DefaultHttpExceptionStatusCodeProvider>();
        return services;
    }

    public static IServiceCollection AddCustomMvc(this IServiceCollection services)
    {
        services
            .AddControllers()
            .ConfigureApiBehaviorOptions(options =>
                options.InvalidModelStateResponseFactory = context =>
                {
                    var error = ApiErrors.BadRequest(context.ModelState);
                    return new BadRequestObjectResult(error)
                    {
                        ContentTypes = { "application/json"}
                    };
                })
            .AddJsonOptions(opt => opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        services.AddControllersWithViews();

        return services;
    }

    public static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddVersionedApiExplorer(setup =>
        {
            setup.GroupNameFormat = "'v'VVV";
            setup.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, fileName));
        });
        services.ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }

    public static IServiceCollection AddCheckoutHealthChecks(this IServiceCollection services)
    {
        var hcBuilder = services.AddHealthChecks();
        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
        return services;
    }

    private static void AddRpcServiceSettings(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        services
            .AddOptions<RpcServiceSettings>(serviceName)
            .Bind(configuration.GetSection("Services:" + serviceName))
            .ValidateDataAnnotations();
    }
}