using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PaymentGateway.Checkout.ApiModels;

public record CreateInvoiceRequest
{
    /// <summary>
    /// Unique identifier for the shop.
    /// </summary>
    [JsonPropertyName("shop_id")]
    [Required]
    public string ShopId { get; init; }

    /// <summary>
    /// Invoice amount as a positive number.
    /// </summary>
    [JsonPropertyName("amount")] 
    [Required]
    public double Amount { get; init; }

    /// <summary>
    /// Currency of the invoice. Three-letter in uppercase, ISO 4217.
    /// </summary>
    [JsonPropertyName("currency")]
    [Required]
    public string Currency { get; init; }

    /// <summary>
    /// Invoice description. Max 1000 characters.
    /// </summary>
    [JsonPropertyName("description")]
    [Required]
    public string Description { get; init; }
}