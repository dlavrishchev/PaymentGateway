namespace PaymentGateway.Billing.Application.PaymentSystems;

public class Decline
{
    public string Code { get; }
    public string Reason { get; }

    public Decline(string code, string reason)
    {
        Code = code;
        Reason = reason;
    }

    public override string ToString()
    {
        return $"{Code}:{Reason}";
    }

    public static Decline InsufficientFunds => new("PS001", "card has insufficient funds to complete the purchase");
    public static Decline LostCard => new("PS002", "card was lost");
    public static Decline StolenCard => new("PS003", "card was stolen");
}