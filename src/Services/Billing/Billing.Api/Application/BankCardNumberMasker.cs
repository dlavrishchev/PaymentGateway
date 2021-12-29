using System;

namespace PaymentGateway.Billing.Application;

public class BankCardNumberMasker : IBankCardNumberMasker
{
    private readonly char _maskChar = '*';

    public string Mask(ReadOnlySpan<char> number)
    {
        // 4000002760003184 -> 400000******3184

        if(number == null)
            throw new ArgumentNullException(nameof(number));

        if(number.Length < 13 || number.Length > 19)
            throw new ArgumentException("Invalid card number length", nameof(number));

        Span<char> buffer = stackalloc char[number.Length];
        number.CopyTo(buffer);

        const int MASK_REGION_START_INDEX = 6;
        var maskRegionEndIndex = number.Length - 5;

        for (var i = MASK_REGION_START_INDEX; i <= maskRegionEndIndex; i++)
            buffer[i] = _maskChar;

        return new string(buffer);
    }
}