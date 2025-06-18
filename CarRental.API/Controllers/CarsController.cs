using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarRental.Application.DTOs;
using CarRental.Application.Services.Interfaces;
using CarRental.Application.DTOs.CarDTOs;

namespace CarRental.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;

        public CarsController(ICarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = null)
        {
            var response = await _carService.GetCarsAsync(pageNumber, pageSize, searchTerm);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var response = await _carService.GetCarByIdAsync(id);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableCars([FromQuery] CarAvailabilityDto availability)
        {
            var response = await _carService.GetAvailableCarsAsync(availability);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{id}/availability")]
        public async Task<IActionResult> CheckAvailability(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var response = await _carService.CheckAvailabilityAsync(id, startDate, endDate);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarDto createCarDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _carService.CreateCarAsync(createCarDto);
            return response.Success ? CreatedAtAction(nameof(GetCar), new { id = response.Data.Id }, response) : BadRequest(response);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDto updateCarDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            updateCarDto.Id = id;
            var response = await _carService.UpdateCarAsync(updateCarDto);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var response = await _carService.DeleteCarAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}