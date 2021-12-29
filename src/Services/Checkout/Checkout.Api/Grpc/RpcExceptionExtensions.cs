using Grpc.Core;

namespace PaymentGateway.Checkout.Grpc;

static class RpcExceptionExtensions
{
    public static string GetErrorCode(this RpcException exception)
    {
        return exception.Trailers.GetErrorCode();
    }

    public static string GetErrorMessage(this RpcException exception)
    {
        return exception.Status.Detail;
    }
}