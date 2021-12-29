using System;
using Grpc.Core;
using PaymentGateway.Checkout.ApiModels;
using PaymentGateway.Checkout.Grpc;

namespace PaymentGateway.Checkout.Infrastructure;

internal class DefaultExceptionToApiErrorConverter : IExceptionToApiErrorConverter
{
    public ApiErrorResponse Convert(Exception ex)
    {
        if(ex is RpcException rpcException)
            return ConvertRpcException(rpcException);

        return ConvertGenericException();
    }

    private static ApiErrorResponse ConvertGenericException()
    {
        return ApiErrors.InternalError;
    }

    private static ApiErrorResponse ConvertRpcException(RpcException ex)
    {
        switch (ex.StatusCode)
        {
            case StatusCode.DeadlineExceeded:
            case StatusCode.Unavailable:
                return ApiErrors.GatewayTimeout;

            default:
                return new ApiErrorResponse(ex.GetErrorCode(), ex.GetErrorMessage());
        }
    }
}