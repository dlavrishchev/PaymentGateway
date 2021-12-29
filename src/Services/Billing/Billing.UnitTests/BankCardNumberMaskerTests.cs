using PaymentGateway.Billing.Application;
using Xunit;

namespace Billing.UnitTests;

public class BankCardNumberMaskerTests
{
    [Fact]
    public void Mask_ValidNumber_ShouldReturnMaskedNumber()
    {
        var expected = "400000******3184";

        var actual = new BankCardNumberMasker().Mask("4000002760003184");

        Assert.Equal(expected, actual);
    }
}