using System.Text.Json.Serialization;

namespace PaymentGateway.Checkout.ApiModels;

public readonly record struct ApiErrorResponse(
    [property: JsonPropertyName("error_code")] 
    string ErrorCode,

    [property: JsonPropertyName("message")] 
    string Message,
        
    [property: JsonPropertyName("details")] 
    string Details = null);