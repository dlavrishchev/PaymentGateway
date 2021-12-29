using PaymentGateway.Billing.Domain;
using Xunit;

namespace Billing.UnitTests;

public class ResultTests
{
    [Fact]
    public void Success_result_for_success_factory_call()
    {
        var result = Result.Success();

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Fail_result_for_fail_factory_call()
    {
        var error = new Error("errorCode", "errorMessage");

        var result = Result.Fail(error);

        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal(error, result.Error);
    }
}