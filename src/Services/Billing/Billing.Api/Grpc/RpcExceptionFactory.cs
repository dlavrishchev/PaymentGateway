using System;
using Grpc.Core;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Grpc;

internal static class RpcExceptionFactory
{
    public static RpcException FromException(Exception exception, StatusCode statusCode)
    {
        Metadata metadata = null;
        if (exception is IHasErrorCode exceptionWihErrorCode)
            metadata = new Metadata().WithErrorCode(exceptionWihErrorCode.ErrorCode);
        return CreateRpcException(exception.Message, statusCode, metadata);
    }

    public static RpcException ToInternalError()
    {
        var (errorCode, message) = Errors.InternalError;
        var metadata = new Metadata().WithErrorCode(errorCode);
        return CreateRpcException(message, StatusCode.Internal, metadata);
    }

    private static RpcException CreateRpcException(string message, StatusCode statusCode, Metadata metadata = null)
    {
        return metadata != null 
            ? new RpcException(new Status(statusCode, message), metadata) 
            : new RpcException(new Status(statusCode, message));
    }
}