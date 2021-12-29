using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PaymentGateway.Checkout.ApiModels;

namespace PaymentGateway.Checkout;

public static class ApiErrors
{
    public static ApiErrorResponse InternalError => new("C0000", "Internal error. Sorry, try later.");
    public static ApiErrorResponse GatewayTimeout => new("C0001", "One of the services is not responding. Sorry, try later.");

    public static ApiErrorResponse BadRequest(ModelStateDictionary modelState)
    {
        const string ERROR_CODE = "C0002";
        const string ERROR_MSG = "Validation failed.";

        var propertyName = modelState.Keys.FirstOrDefault();
        if (propertyName == null)
            return new ApiErrorResponse(ERROR_CODE, ERROR_MSG);

        var normalizedPropertyName = propertyName.TrimStart('.', '$');
        if(string.IsNullOrEmpty(normalizedPropertyName))
            return new ApiErrorResponse(ERROR_CODE, ERROR_MSG, "Invalid request format.");

        var details = $"{normalizedPropertyName}: invalid value";
        return new ApiErrorResponse(ERROR_CODE, ERROR_MSG, details);
    }
}