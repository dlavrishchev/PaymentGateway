using System;
using FluentValidation;

namespace PaymentGateway.Billing.Application.Extensions;

internal static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string> ValidGuid<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Must(input => Guid.TryParse(input, out _));
    }

    public static IRuleBuilderOptions<T, string> ValidMongoId<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.Matches(@"^[0-9a-fA-F]{24}$");
    }
}