using Microsoft.AspNetCore.Mvc;

namespace PaymentGateway.Checkout.Controllers;

[Route("/statuscode")]
[ApiExplorerSettings(IgnoreApi = true)]
public class StatusCodeController : Controller
{
    [HttpGet("404")]
    public IActionResult PageNotFound()
    {
        ViewData["Code"] = "404";
        ViewData["Message"] = "PAGE NOT FOUND";

        return View();
    }
}