using Grpc.Core;

namespace PaymentGateway.Checkout.Grpc;

internal static class MetadataExtensions
{
    public static string GetErrorCode(this Metadata metadata)
    {
        var entry = metadata.Get("error_code");
        return entry?.Value;
    }
}