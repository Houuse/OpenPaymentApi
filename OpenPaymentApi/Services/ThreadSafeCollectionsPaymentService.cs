using System.Collections.Concurrent;
using Microsoft.Extensions.Options;

namespace OpenPaymentApi.Services;

public class ThreadSafeCollectionsPaymentService : IPaymentService
{
    private readonly ConcurrentBag<RegisteredPayment> _payments = new();
    private readonly ConcurrentDictionary<string, object> _paymentRequestsInProgress = new();
    private readonly ServerSettingsOptions _serverSettings;

    public ThreadSafeCollectionsPaymentService(IOptions<ServerSettingsOptions> serverSettingsOption)
    {
        _serverSettings = serverSettingsOption.Value;
    }

    public async Task<Result<string>> RegisterPayment(string clientId, PaymentRequest paymentRequest)
    {
        var paymentInProgress = _paymentRequestsInProgress.ContainsKey(clientId);
        if (paymentInProgress)
        {
            return Result<string>.Error("Payment already in progress for the client");
        }

        _paymentRequestsInProgress[clientId] = new object();
        Thread.Sleep(TimeSpan.FromSeconds(_serverSettings.PaymentProcessingTimeInSeconds));

        var payment = new RegisteredPayment
        {
            Currency = paymentRequest.Currency,
            Creditor_Account = paymentRequest.Creditor_Account,
            Debtor_Account = paymentRequest.Debtor_Account,
            Transaction_Amount = paymentRequest.Instructed_Amount,
            Payment_ID = Guid.NewGuid().ToString(),
        };
        _payments.Add(payment);
        _paymentRequestsInProgress.Remove(clientId, out _);
        return Result<string>.Success(payment.Payment_ID);
    }

    public async Task<Result<RegisteredPayment[]>> GetPayments(string iban)
    {
        var result = _payments.Where(p => p.Creditor_Account == iban || p.Debtor_Account == iban).ToArray();
        return Result<RegisteredPayment[]>.Success(result);
    }
}