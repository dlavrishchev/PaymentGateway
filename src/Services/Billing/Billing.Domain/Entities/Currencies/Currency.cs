namespace PaymentGateway.Billing.Domain.Entities.Currencies;

public class Currency : Entity
{
    public string Code { get; private set; }
    public string Name { get; private set; }

    public Currency(string code, string name)
    {
        Guard.NotNull(code, nameof(code));
        Guard.NotNullOrWhiteSpace(name, nameof(name));
            
        Code = code;
        Name = name;
    }
}