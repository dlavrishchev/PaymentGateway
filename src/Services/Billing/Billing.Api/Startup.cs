using Billing.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Billing.Application.Extensions;
using PaymentGateway.Billing.Grpc;
using PaymentGateway.Billing.Grpc.Interceptors;

namespace PaymentGateway.Billing;

public class Startup
{
    public IConfiguration Configuration { get; }

        
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddOptions();
            
        services.AddGrpc(options =>
        {
            options.Interceptors.Add<ExceptionHandlingGrpcInterceptor>();
            //options.EnableDetailedErrors = true;
        });

        services
            .AddMongoDb(Configuration)
            .AddBillingServices()
            .AddBillingHealthChecks();

    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/healthz");
            endpoints.MapGrpcService<BillingService>();
        });
    }
}