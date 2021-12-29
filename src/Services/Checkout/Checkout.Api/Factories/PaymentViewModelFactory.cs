using System.Linq;
using PaymentGateway.Checkout.ViewModels;
using PaymentGateway.Grpc.Billing;

namespace PaymentGateway.Checkout.Factories;

public class PaymentViewModelFactory
{
    private readonly PaymentViewModelDropDownItemsProvider _dropDownItemsProvider;

    public PaymentViewModelFactory(PaymentViewModelDropDownItemsProvider dropDownItemsProvider)
    {
        _dropDownItemsProvider = dropDownItemsProvider;
    }

    public PaymentPageViewModel PreparePaymentPageViewModel(PaymentFormData formData)
    {
        return new PaymentPageViewModel
        {
            InvoiceDescription = formData.Description,
            InvoiceTotalAmount = $"{formData.Amount} {formData.Currency}",
            ButtonCaption = $"Pay {formData.Amount} {formData.Currency}",
            PaymentMethodId = formData.PaymentMethods.First().Id, // todo: process multiple payment methods
            IsTestTransaction = formData.IsTestTransaction,
            ShopName = formData.ShopName,
            Months = _dropDownItemsProvider.AvailableMonths(),
            Years = _dropDownItemsProvider.AvailableYears()
        };
    }

    public PaymentResultViewModel PreparePaymentResultViewModel(ProcessPaymentResponse paymentResult)
    {
        return new PaymentResultViewModel
        {
            RedirectButtonCaption = "Return to merchant site",
            IsSuccess = paymentResult.IsSuccess,
            Status = paymentResult.IsSuccess ? "Success" : "Declined",
            RedirectUrl = paymentResult.IsSuccess ? paymentResult.SuccessUrl : paymentResult.FailUrl,
            TransactionId = $"Tnx id: {paymentResult.TransactionId}",
            Rrn = $"RRN: {paymentResult.Rrn}",
            CardNumber = $"PAN: {paymentResult.MaskedCardNumber}",
            DeclineCode = paymentResult.IsSuccess ? null : $"Decline code: {paymentResult.DeclineCode}",
            DeclineReason = paymentResult.IsSuccess ? null : $"Decline reason: {paymentResult.DeclineReason}"
        };
    }
}