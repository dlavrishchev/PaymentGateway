namespace PaymentGateway.Billing.Domain.Entities;

public abstract class Entity
{
    public string Id { get; set; }

    public override string ToString()
    {
        return !string.IsNullOrEmpty(Id) ? $"{EntityName}_{Id}" : $"{EntityName}";
    }

    private string EntityName => GetType().Name;
}