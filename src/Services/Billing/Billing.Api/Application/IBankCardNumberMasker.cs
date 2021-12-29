using System;

namespace PaymentGateway.Billing.Application;

public interface IBankCardNumberMasker
{
    string Mask(ReadOnlySpan<char> number);
}