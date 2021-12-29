using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Checkout.Factories;
using PaymentGateway.Checkout.Services;
using PaymentGateway.Checkout.ViewModels;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Checkout.Controllers;

[Route("pay/invoice/{invoiceGuid}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class PaymentController : Controller
{
    private readonly IBillingService _billingService;
    private readonly PaymentViewModelFactory _modelFactory;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(IBillingService billingService, PaymentViewModelFactory modelFactory, ILogger<PaymentController> logger)
    {
        _billingService = billingService;
        _modelFactory = modelFactory;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Pay(string invoiceGuid)
    {
        var request = new GetPaymentFormDataRequest {InvoiceGuid = invoiceGuid};
        var formData = await _billingService.GetPaymentFormDataAsync(request);
        var model = _modelFactory.PreparePaymentPageViewModel(formData);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Pay([FromForm] PaymentPageViewModel model, string invoiceGuid)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid PaymentPageViewModel state: {Errors}", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest();
        }

        var request = new ProcessPaymentRequest
        {
            InvoiceGuid = invoiceGuid,
            PaymentMethodId = model.PaymentMethodId,
            BankCardRequisites = new BankCardRequisites
            {
                Holder = model.CardData.Holder,
                Number = model.CardData.Number,
                ExpiryMonth = model.CardData.ExpiryMonth,
                ExpiryYear = model.CardData.ExpiryYear,
                Cvv = model.CardData.Cvv
            }
        };

        var paymentResult = await _billingService.ProcessPaymentAsync(request);
        var resultModel = _modelFactory.PreparePaymentResultViewModel(paymentResult);
        return View("PaymentResult", resultModel);
    }

        
}