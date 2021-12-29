using System.Text.Json.Serialization;

namespace PaymentGateway.Checkout.ApiModels;

public record CreateInvoiceResponse(
    [property: JsonPropertyName("invoice_id")] string InvoiceGuid,
    [property: JsonPropertyName("payment_page_url")] string PaymentPageUrl);