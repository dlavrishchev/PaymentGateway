using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Checkout.ViewModels;

public class BankCardViewModel
{
    [Required]
    [MaxLength(50, ErrorMessage = "Card holder value is too long")]
    [DisplayName("Card holder")]
    public string Holder { get; set; }

    [Required]
    [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Invalid card format")]
    [DisplayName("Card number")]
    public string Number { get; set; }

    [Required]
    [RegularExpression(@"(0[1-9]|1[0-2])", ErrorMessage = "Invalid card expiry month")]
    [DisplayName("Card expiry month")]
    public string ExpiryMonth { get; set; }

    [Required]
    [RegularExpression(@"[0-9]{2}", ErrorMessage = "Invalid card expiry year")]
    [DisplayName("Card expiry year")]
    public string ExpiryYear { get; set; }

    [Required]
    [RegularExpression(@"[0-9]{3}", ErrorMessage = "Invalid card CVV")]
    [DisplayName("Card CVV")]
    public string Cvv { get; set; }
}