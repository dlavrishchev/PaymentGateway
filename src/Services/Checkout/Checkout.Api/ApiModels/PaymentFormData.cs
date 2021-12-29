using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace PaymentGateway.Checkout.ApiModels;

public class PaymentFormData
{
    /// <summary>
    /// Unique identifier for the invoice.
    /// </summary>
    [JsonPropertyName("invoice_id")]
    public string InvoiceGuid { get; set; }

    /// <summary>
    /// Invoice total amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public double Amount { get; set; }

    /// <summary>
    /// Currency of the invoice.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; }

    /// <summary>
    /// Invoice description.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// Shop`s name.
    /// </summary>
    [JsonPropertyName("shop_name")]
    public string ShopName { get; set; }

    /// <summary>
    /// Has a true value for a test payment that goes through a test sandbox.
    /// </summary>
    [JsonPropertyName("is_test_transaction")]
    public bool IsTestTransaction { get; set; }

    [JsonPropertyName("payment_methods")]
    public List<PaymentMethod> PaymentMethods { get; }

    public PaymentFormData()
    {
        PaymentMethods = new List<PaymentMethod>();
    }
}

public class PaymentMethod
{
    /// <summary>
    /// Unique identifier for the payment method.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Payment method`s name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}