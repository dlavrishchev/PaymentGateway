namespace PaymentGateway.Billing.Domain.Entities.Shops;

public class ShopRedirect : ValueObject
{
    /// <summary>
    /// URL to redirect a customer after the failed payment.
    /// </summary>
    public string FailUrl { get; private set; }

    /// <summary>
    /// URL to redirect a customer after the successful payment.
    /// </summary>
    public string SuccessUrl { get; private set; }

    public ShopRedirect(string failUrl, string successUrl)
    {
        Guard.NotNullOrWhiteSpace(failUrl, nameof(failUrl));
        Guard.NotNullOrWhiteSpace(successUrl, nameof(successUrl));

        FailUrl = failUrl;
        SuccessUrl = successUrl;
    }
}