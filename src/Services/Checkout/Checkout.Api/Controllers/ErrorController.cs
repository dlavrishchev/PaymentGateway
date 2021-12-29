using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Checkout.Infrastructure;

namespace PaymentGateway.Checkout.Controllers;

[Route("/error")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : Controller
{
    private readonly IExceptionToApiErrorConverter _exceptionToApiErrorConverter;
    private readonly IHttpExceptionStatusCodeProvider _httpExceptionStatusCodeProvider;

    public ErrorController(IExceptionToApiErrorConverter exceptionToApiErrorConverter, IHttpExceptionStatusCodeProvider httpExceptionStatusCodeProvider)
    {
        _exceptionToApiErrorConverter = exceptionToApiErrorConverter;
        _httpExceptionStatusCodeProvider = httpExceptionStatusCodeProvider;
    }

    public IActionResult Index()
    {
        var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = exceptionHandlerFeature.Error;
        var apiError = _exceptionToApiErrorConverter.Convert(exception);
        var httpCode = _httpExceptionStatusCodeProvider.GetStatusCode(exception);

        HttpContext.Response.StatusCode = (int)httpCode;
        return new ObjectResult(apiError);
    }
}