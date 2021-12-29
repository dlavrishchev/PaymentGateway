using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Checkout.Controllers;

[ApiController]
[Produces("application/json")]
//[TypeFilter(typeof(ApiExceptionFilter))]
public class ApiBaseController : ControllerBase
{
}