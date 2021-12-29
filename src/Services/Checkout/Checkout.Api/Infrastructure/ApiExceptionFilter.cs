using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Checkout.Infrastructure;

internal class ApiExceptionFilter : IExceptionFilter
{
    private readonly IExceptionToApiErrorConverter _exceptionToApiErrorConverter;
    private readonly IHttpExceptionStatusCodeProvider _httpExceptionStatusCodeProvider;
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(IExceptionToApiErrorConverter exceptionToApiErrorConverter, 
                              IHttpExceptionStatusCodeProvider httpExceptionStatusCodeProvider,
                              ILogger<ApiExceptionFilter> logger)
    {
        _exceptionToApiErrorConverter = exceptionToApiErrorConverter;
        _httpExceptionStatusCodeProvider = httpExceptionStatusCodeProvider;
        _logger = logger;
    }
    public void OnException(ExceptionContext context)
    {
        var apiError = _exceptionToApiErrorConverter.Convert(context.Exception);
        var httpCode = _httpExceptionStatusCodeProvider.GetStatusCode(context.Exception);
        Log(httpCode, context.Exception);

        context.HttpContext.Response.StatusCode = (int)httpCode;
        context.Result = new ObjectResult(apiError);
        context.ExceptionHandled = true;
    }

    private void Log(HttpStatusCode httpStatusCode, Exception exception)
    {
        var statusCodeInt = (int)httpStatusCode;
        if(statusCodeInt is >= 500 and < 600)
            _logger.LogError("Error occured: @{Exception}", exception);
    }
}