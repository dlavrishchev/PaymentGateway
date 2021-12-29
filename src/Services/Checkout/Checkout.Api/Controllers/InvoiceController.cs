using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Checkout.ApiModels;
using PaymentGateway.Checkout.Extensions;
using PaymentGateway.Checkout.Services;

namespace PaymentGateway.Checkout.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/invoices")]
public class InvoiceController : ApiBaseController
{
    private readonly IBillingService _billingService;

    public InvoiceController(IBillingService billingService)
    {
        _billingService = billingService;
    }
        
    /// <summary>
    /// Create a payment invoice
    /// </summary>
    /// <response code="200">Returns the invoice id and payment form url</response>
    /// <response code="400">The error code and message with the error details</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateInvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateInvoiceResponse>> CreateInvoiceAsync([FromBody] CreateInvoiceRequest request)
    {
        var grpcRequest = new PaymentGateway.Grpc.Billing.CreateInvoiceRequest
        {
            ShopId = request.ShopId,
            Description = request.Description,
            Amount = request.Amount,
            Currency = request.Currency
        };
        var invoice = await _billingService.CreateInvoiceAsync(grpcRequest);
        var paymentPageUrl = CreatePaymentPageUrl(invoice.Guid);

        return Ok(new CreateInvoiceResponse(invoice.Guid, paymentPageUrl));
    }

    /// <summary>
    /// Get the invoice data to render a payment form
    /// </summary>
    /// <param name="invoiceId">Unique identifier for the invoice</param>
    /// <response code="200">Returns the invoice data to render a payment form</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="404">Not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("{invoiceId}")]
    [ProducesResponseType(typeof(PaymentFormData), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaymentFormData>> GetPaymentFormData([Required]string invoiceId)
    {
        var request = new PaymentGateway.Grpc.Billing.GetPaymentFormDataRequest {InvoiceGuid = invoiceId};
        var grpcFormData = await _billingService.GetPaymentFormDataAsync(request);
        return Ok(grpcFormData.ToApiModel());
    }

    private string CreatePaymentPageUrl(string invoiceGuid)
    {
        return Url.Action("Pay", "Payment", new { invoiceGuid }, Request.Scheme);
    }
}