using AtmiraPayNet.Server.Services.Interfaces;
using AtmiraPayNet.Shared.DTO;
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

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpPut]
        async public Task<IActionResult> UpdatePayment([FromQuery] Guid id, [FromBody] PaymentDTO request)
        {
            var response = await _paymentService.UpdatePayment(id, request);

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet]
        async public Task<IActionResult> GetPaymentById([FromQuery] Guid id)
        {
            var response = await _paymentService.GetPaymentById(id);

            if (response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, response.Value);
            }

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet]
        [Route("all")]
        async public Task<IActionResult> GetPaymentList()
        {
            var response = await _paymentService.GetPaymentList();

            if (response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, response.Value);
            }

            return StatusCode(response.StatusCode, new { response.Message });
        }

        [HttpGet]
        [Route("pdf")]
        async public Task<IActionResult> GetPaymentPDF([FromQuery] Guid id)
        {
            var response = await _paymentService.GetPaymentPDF(id);

            if (response.StatusCode == 200)
            {
                return StatusCode(response.StatusCode, response.Value);
            }

            return StatusCode(response.StatusCode, new { response.Message });
        }
    }
}
