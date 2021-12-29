namespace PaymentGateway.Billing.Domain.Entities.Shops;

public class ShopWebhookNotification : ValueObject
{
    // Secret key for sign webhook notification requests.
    public string SecretKey { get; private set; }

    // URL to get webhooks.
    public string NotificationUrl { get; private set; }

    public ShopWebhookNotification(string secretKey, string notificationUrl)
    {
        Guard.NotNullOrWhiteSpace(secretKey, nameof(secretKey));
        Guard.NotNullOrWhiteSpace(notificationUrl, nameof(notificationUrl));

        SecretKey = secretKey;
        NotificationUrl = notificationUrl;
    }
}