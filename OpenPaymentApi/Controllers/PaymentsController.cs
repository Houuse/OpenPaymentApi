using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenPaymentApi.Services;

namespace OpenPaymentApi.Controllers
{
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [Route("/payments")]
        public async Task<IActionResult> PostPayment([FromHeader(Name = "Client-ID")] string clientId,
            [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Disallow)] PaymentRequest paymentRequest)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                return BadRequest();

            var paymentResult = await _paymentService.RegisterPayment(clientId, paymentRequest);
            if (paymentResult.IsSuccess)
            {
                return Ok(paymentResult.Data);
            }

            return Conflict(paymentResult.ErrorMessage);
        }

        [HttpGet]
        [Route("/accounts/{iban}/transactions")]
        public async Task<IActionResult> GetPayments(string iban)
        {
            var payments = await _paymentService.GetPayments(iban);
            if (payments.IsSuccess)
            {
                if (payments.Data.Length != 0)
                {
                    return Ok(payments.Data);
                }

                return NoContent();
            }

            return Problem(payments.ErrorMessage);
        }
    }
}