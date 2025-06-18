using CarRental.Application.DTOs.PaymentDTOs;
using CarRental.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    // PaymentsController.cs
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("rental/{rentalId}")]
        public async Task<IActionResult> GetPaymentsByRental(int rentalId)
        {
            var response = await _paymentService.GetPaymentsByRentalAsync(rentalId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment([FromBody] CreatePaymentDto paymentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _paymentService.ProcessPaymentAsync(paymentDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("{id}/refund")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> RefundPayment(int id, [FromBody] decimal amount)
        {
            var response = await _paymentService.RefundPaymentAsync(id, amount);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }

}
