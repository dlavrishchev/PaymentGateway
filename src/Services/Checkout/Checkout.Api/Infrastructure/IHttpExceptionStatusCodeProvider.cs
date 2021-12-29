using System;
using System.Net;

namespace PaymentGateway.Checkout.Infrastructure;

public interface IHttpExceptionStatusCodeProvider
{
    HttpStatusCode GetStatusCode(Exception exception);
}