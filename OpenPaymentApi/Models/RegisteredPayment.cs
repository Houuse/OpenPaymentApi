namespace OpenPaymentApi;

public class RegisteredPayment
{
    public string Payment_ID { get; set; }
    public string Debtor_Account { get; set; }
    public string Creditor_Account { get; set; }
    public string Transaction_Amount { get; set; }
    public string Currency { get; set; }
}