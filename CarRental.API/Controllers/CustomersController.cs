using CarRental.Application.DTOs.CustomerDTOs;
using CarRental.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetCustomers()
        {
            var response = await _customerService.GetAllCustomersAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto createCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _customerService.CreateCustomerAsync(createCustomerDto);
            return response.Success ? CreatedAtAction(nameof(GetCustomer), new { id = response.Data.Id }, response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CreateCustomerDto updateCustomerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _customerService.UpdateCustomerAsync(id, updateCustomerDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("{id}/verify")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> VerifyCustomer(int id)
        {
            var response = await _customerService.VerifyCustomerAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }

}
