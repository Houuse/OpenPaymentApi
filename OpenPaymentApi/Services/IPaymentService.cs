namespace OpenPaymentApi.Services;

public interface IPaymentService
{
    Task<Result<string>> RegisterPayment(string clientId, PaymentRequest paymentRequest);
    Task<Result<RegisteredPayment[]>> GetPayments(string iban);
}