namespace PaymentGateway.Billing.Application.PaymentSystems;

public class ProcessPaymentResult
{
    public string Rrn { get; private set; }
    public string TransactionId { get; private set; }
    public bool IsSuccess { get; private set; }
    public Decline Decline { get; private set; }

    public ProcessPaymentResult(string transactionId, string rrn)
    {
        TransactionId = transactionId;
        Rrn = rrn;
        IsSuccess = true;
    }

    public ProcessPaymentResult(string transactionId, Decline decline)
    {
        TransactionId = transactionId;
        Decline = decline;
        IsSuccess = false;
    }
}