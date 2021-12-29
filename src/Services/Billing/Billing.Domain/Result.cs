namespace PaymentGateway.Billing.Domain;

public class Result
{
    public Error Error { get; init; }
    public bool IsSuccess => Error == null;

    private Result()
    {
    }

    private Result(Error error)
    {
        Guard.NotNull(error, nameof(error));
        Error = error;
    }

    public override string ToString()
    {
        return IsSuccess ? "success result" : Error.ToString();
    }

    public static Result Success() => new();
    public static Result Fail(Error error) => new(error);
}