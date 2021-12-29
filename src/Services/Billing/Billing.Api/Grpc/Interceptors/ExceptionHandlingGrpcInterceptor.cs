using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using PaymentGateway.Billing.Application.Exceptions;
using PaymentGateway.Billing.Domain.Exceptions;

namespace PaymentGateway.Billing.Grpc.Interceptors;

public class ExceptionHandlingGrpcInterceptor : Interceptor
{
    private readonly ILogger<ExceptionHandlingGrpcInterceptor> _logger;

    public ExceptionHandlingGrpcInterceptor(ILogger<ExceptionHandlingGrpcInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (ValidationException ex)
        {
            throw RpcExceptionFactory.FromException(ex, StatusCode.InvalidArgument); //400
        }
        catch (BadRequestException ex)
        {
            throw RpcExceptionFactory.FromException(ex, StatusCode.InvalidArgument); //400
        }
        catch (ResourceNotFoundException ex)
        {
            throw RpcExceptionFactory.FromException(ex, StatusCode.NotFound); //404
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while processing request {RequestType}. Exception: @{Exception}", request.GetType(), ex);
            throw RpcExceptionFactory.ToInternalError();
        }
    }
}