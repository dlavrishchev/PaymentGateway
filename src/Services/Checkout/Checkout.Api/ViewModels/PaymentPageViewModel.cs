using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PaymentGateway.Checkout.ViewModels;

public class PaymentPageViewModel
{
    [Required]
    public string PaymentMethodId { get; set; }
    public string InvoiceDescription { get; set; }
    public string InvoiceTotalAmount { get; set; }
    public string ButtonCaption { get; set; }
    public bool IsTestTransaction { get; set; }
    public string ShopName { get; set; }

    public BankCardViewModel CardData { get; set; }
    public IEnumerable<SelectListItem> Months { get; set; }
    public IEnumerable<SelectListItem> Years { get; set; }
}