using System;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Application.Exceptions;

internal class BadRequestException : Exception, IHasErrorCode
{
    public string ErrorCode { get; private set; }

    public BadRequestException(string message, string errorCode) : base(message)
    {
        Guard.NotNullOrWhiteSpace(message, nameof(message));
        Guard.NotNullOrWhiteSpace(errorCode, nameof(errorCode));

        ErrorCode = errorCode;
    }
}