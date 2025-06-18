using CarRental.Application.DTOs.RentalDTOs;
using CarRental.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    // RentalsController.cs
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;

        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRental(int id)
        {
            var response = await _rentalService.GetRentalByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("active")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetActiveRentals()
        {
            var response = await _rentalService.GetActiveRentalsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetRentalsByCustomer(int customerId)
        {
            var response = await _rentalService.GetRentalsByCustomerAsync(customerId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalDto createRentalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _rentalService.CreateRentalAsync(createRentalDto);
            return response.Success ? CreatedAtAction(nameof(GetRental), new { id = response.Data.Id }, response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateRental(int id, [FromBody] UpdateRentalDto updateRentalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            updateRentalDto.Id = id;
            var response = await _rentalService.UpdateRentalAsync(updateRentalDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CompleteRental(int id, [FromBody] DateTime returnDate)
        {
            var response = await _rentalService.CompleteRentalAsync(id, returnDate);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelRental(int id)
        {
            var response = await _rentalService.CancelRentalAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("calculate-cost")]
        public async Task<IActionResult> CalculateCost([FromBody] CalculateCostDto calculateCostDto)
        {
            var response = await _rentalService.CalculateRentalCostAsync(
                calculateCostDto.CarId,
                calculateCostDto.StartDate,
                calculateCostDto.EndDate,
                calculateCostDto.AdditionalServiceIds
            );
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }

}
