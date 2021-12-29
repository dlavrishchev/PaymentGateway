using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Checkout.Configuration;

public class RpcServiceSettings
{
    [Required]
    public string Url { get; set; }

    [Range(0, 10_000)]
    public int CallDeadline { get; set; } //ms

    [Required]
    public RetryConfig Retry { get; set; }
}

public class RetryConfig
{
    [Range(1, 10)]
    public int Count { get; set; }
}