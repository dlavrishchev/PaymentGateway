using System.Linq;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Checkout.Extensions;

internal static class GrpcModelExtensions
{
    public static ApiModels.PaymentFormData ToApiModel(this PaymentFormData grpcFormData)
    {
        var apiModel = new ApiModels.PaymentFormData
        {
            InvoiceGuid = grpcFormData.InvoiceGuid,
            Amount = grpcFormData.Amount,
            Currency = grpcFormData.Currency,
            Description = grpcFormData.Description,
            IsTestTransaction = grpcFormData.IsTestTransaction,
            ShopName = grpcFormData.ShopName
        };
        apiModel.PaymentMethods.AddRange(grpcFormData.PaymentMethods.Select(pm => pm.ToApiModel()));
        return apiModel;
    }

    public static ApiModels.PaymentMethod ToApiModel(this PaymentFormPaymentMethod grpcPaymentMethod)
    {
        return new ApiModels.PaymentMethod
        {
            Id = grpcPaymentMethod.Id,
            Name = grpcPaymentMethod.Name
        };
    }
}