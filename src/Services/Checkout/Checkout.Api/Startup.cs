using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Checkout.Extensions;

namespace PaymentGateway.Checkout;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCustomMvc()
            .AddCustomApiVersioning()
            .AddSettings(Configuration)
            .AddBaseServices()
            .AddGrpcClients()
            .AddCheckoutHealthChecks()
            .AddCustomSwagger();
    }

    public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        app.UseExceptionHandler("/error");
        app.UseStatusCodePagesWithReExecute("/statuscode/{0}");
        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/healthz");
        });

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json", 
                    description.GroupName);
            }
        });

            
    }
}