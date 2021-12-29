using System;

namespace PaymentGateway.Billing.Domain.Exceptions;

public class ValidationException : Exception, IHasErrorCode
{
    public string ErrorCode { get; }
    public ValidationException(Error error) : base(error.Message)
    {
        ErrorCode = error.ErrorCode;
    }
}