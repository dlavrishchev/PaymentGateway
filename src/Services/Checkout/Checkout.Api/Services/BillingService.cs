using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PaymentGateway.Checkout.Configuration;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Checkout.Services;

public class BillingService : IBillingService
{
    public const string Name = "Billing";
    private readonly PaymentGateway.Grpc.Billing.BillingService.BillingServiceClient _grpcClient;
    private readonly RpcServiceSettings _settings;

    public BillingService(
        PaymentGateway.Grpc.Billing.BillingService.BillingServiceClient grpcClient, 
        IOptionsSnapshot<RpcServiceSettings> options)
    {
        _settings = options.Get(Name);
        _grpcClient = grpcClient;
    }

    public async Task<Invoice> CreateInvoiceAsync(CreateInvoiceRequest request)
    {
        return await _grpcClient.CreateInvoiceAsync(request, deadline: Deadline);
    }

    public async Task<PaymentFormData> GetPaymentFormDataAsync(GetPaymentFormDataRequest request)
    {
        return await _grpcClient.GetPaymentFormDataAsync(request, deadline: Deadline);
    }

    public async Task<ProcessPaymentResponse> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        return await _grpcClient.ProcessPaymentAsync(request, deadline: Deadline);
    }

    private DateTime Deadline => DateTime.UtcNow.AddMilliseconds(_settings.CallDeadline);
}