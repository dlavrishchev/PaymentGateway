namespace PaymentGateway.Billing.Domain.Entities.Merchants;

public class MerchantCompany : ValueObject
{
    public string Name { get; set; }

    /// <summary>
    /// ОГРН
    /// </summary>
    public string RegisteredNumber { get; set; }

    /// <summary>
    /// ИНН
    /// </summary>
    public string Inn { get; set; }

    /// <summary>
    /// Фактический адрес
    /// </summary>
    public string ActualAddress { get; set; }

    /// <summary>
    /// Юридический адрес
    /// </summary>
    public string LegalAddress { get; set; }
}