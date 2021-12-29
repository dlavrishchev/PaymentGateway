namespace PaymentGateway.Billing.Domain;

public interface IHasErrorCode
{
    string ErrorCode { get; }
}