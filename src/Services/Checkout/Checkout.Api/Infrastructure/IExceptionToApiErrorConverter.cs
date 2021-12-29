using System;
using PaymentGateway.Checkout.ApiModels;

namespace PaymentGateway.Checkout.Infrastructure;

public interface IExceptionToApiErrorConverter
{
    public ApiErrorResponse Convert(Exception ex);
}