using System;
using System.Net;
using Grpc.Core;

namespace PaymentGateway.Checkout.Infrastructure;

internal class DefaultHttpExceptionStatusCodeProvider : IHttpExceptionStatusCodeProvider
{
    public HttpStatusCode GetStatusCode(Exception exception)
    {
        if(exception is RpcException rpcException)
            return GetStatusCodeFromRpcException(rpcException);

        return HttpStatusCode.InternalServerError;
    }

    private static HttpStatusCode GetStatusCodeFromRpcException(RpcException rpcException)
    {
        switch (rpcException.StatusCode)
        {
            case StatusCode.InvalidArgument:
                return HttpStatusCode.BadRequest;

            case StatusCode.DeadlineExceeded:
            case StatusCode.Unavailable:
                return HttpStatusCode.GatewayTimeout;

            case StatusCode.NotFound:
                return HttpStatusCode.NotFound;

            default:
                return HttpStatusCode.InternalServerError;
        }
    }
}