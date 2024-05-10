using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
using AtmiraPayNet.Shared.Utils;
using Microsoft.AspNetCore.Mvc;

namespace AtmiraPayNet.Server.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController(IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;

        [HttpPost]
        async public Task<IActionResult> CreatePayment([FromBody] PaymentDTO request)
        {
            var response = await _paymentService.CreatePayment(request);

            if (response.StatusCode == StatusCodes.Status200OK && request.Status == Status.Generated)
            {
                return StatusCode(response.StatusCode, new { response.Message, pdf = response.Value!.PaymentLetter!.PDF });
            }

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPut]
        async public Task<IActionResult> UpdatePayment([FromBody] PaymentDTO request)
        {
            var response = await _paymentService.UpdatePayment(request);

            if (response.StatusCode == StatusCodes.Status200OK && request.Status == Status.Generated)
            {
                return StatusCode(response.StatusCode, new { response.Message, pdf = response.Value!.PaymentLetter!.PDF });
            }

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet]
        async public Task<IActionResult> GetPaymentById([FromQuery] Guid id)
        {
            var response = await _paymentService.GetPaymentById(id);

            return StatusCode(response.StatusCode, new { response.Message, response.Value });
        }

        [HttpGet]
        [Route("all")]
        async public Task<IActionResult> GetPaymentList()
        {
            var response = await _paymentService.GetPaymentList();

            return StatusCode(response.StatusCode, new { response.Message, response.Value });
        }
    }
}
