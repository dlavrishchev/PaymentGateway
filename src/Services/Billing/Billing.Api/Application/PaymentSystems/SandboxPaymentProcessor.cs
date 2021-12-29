using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Billing.Application.PaymentSystems;

public class SandboxPaymentProcessor : IPaymentProcessor
{
    private readonly Random _random = new();

    private readonly Dictionary<string, Decline> _declinedCards = new()
    {
        {"4000000000009995", Decline.LostCard},
        {"4000000000000069", Decline.InsufficientFunds},
        {"4000000000000127", Decline.StolenCard}
    };

    public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest request)
    {
        if (IsNumberDeclined(request.CardData.Number, out var decline))
            return await Task.FromResult(CreateDeclinedResponse(decline));

        return await Task.FromResult(CreateAuthorizedResponse());
    }

    private bool IsNumberDeclined(string cardNumber, out Decline decline)
    {
        return _declinedCards.TryGetValue(cardNumber, out decline);
    }

    private ProcessPaymentResult CreateDeclinedResponse(Decline decline)
    {
        return new ProcessPaymentResult(GenerateTransactionId(), decline);
    }

    private ProcessPaymentResult CreateAuthorizedResponse()
    {
        return new ProcessPaymentResult(GenerateTransactionId(), GenerateRrn());
    }

    private string GenerateRrn()
    {
        return GenerateRandomString(6);
    }

    private string GenerateTransactionId()
    {
        return GenerateRandomString(10);
    }

    private string GenerateRandomString(int length)
    {
        return new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}