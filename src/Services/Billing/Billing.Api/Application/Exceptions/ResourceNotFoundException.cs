using System;
using PaymentGateway.Billing.Domain;

namespace PaymentGateway.Billing.Application.Exceptions;

class ResourceNotFoundException : Exception, IHasErrorCode
{
    public string ErrorCode { get; }
    public ResourceNotFoundException(Error error) : base(error.Message)
    {
        ErrorCode = error.ErrorCode;
    }
}