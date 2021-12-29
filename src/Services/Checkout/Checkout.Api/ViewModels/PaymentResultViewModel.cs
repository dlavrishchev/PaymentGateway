namespace PaymentGateway.Checkout.ViewModels;

public class PaymentResultViewModel
{
    public bool IsSuccess { get; set; }
    public string Status { get; set; }
    public string RedirectButtonCaption { get; set; }
    public string RedirectUrl { get; set; }
    public string TransactionId { get; set; }
    public string Rrn { get; set; }
    public string CardNumber { get; set; }
    public string DeclineCode { get; set; }
    public string DeclineReason { get; set; }
}