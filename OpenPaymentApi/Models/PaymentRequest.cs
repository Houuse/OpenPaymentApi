using System.ComponentModel.DataAnnotations;

namespace OpenPaymentApi;

public class PaymentRequest
{
    [Required] [Length(1, 34)] public string Debtor_Account { get; set; }

    [Required] [Length(1, 34)] public string Creditor_Account { get; set; }

    [Required]
    [RegularExpression("-?[0-9]{1,14}(\\.[0-9]{1,3})?")]
    public string Instructed_Amount { get; set; }

    [Required] [Length(3, 3)] public string Currency { get; set; }
}