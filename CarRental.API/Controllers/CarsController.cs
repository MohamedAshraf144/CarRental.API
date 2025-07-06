using CarRental.Application.DTOs;
using CarRental.Application.DTOs.CarDTOs;
using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        /// <summary>
        /// Get a paginated list of cars
        /// </summary>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <param name="searchTerm">Search term for filtering</param>
        /// <returns>Paginated list of cars</returns>
        /// <response code="200">Returns the list of cars</response>
        /// <response code="400">If the request is invalid</response>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<CarDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<PagedResponse<CarDto>>), 400)]
        public async Task<IActionResult> GetCars([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = null)
        {
            var response = await _carService.GetCarsAsync(pageNumber, pageSize, searchTerm);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        // <summary>
        /// Get a specific car by ID
        /// </summary>
        /// <param name="id">Car ID</param>
        /// <returns>Car details</returns>
        /// <response code="200">Returns the car details</response>
        /// <response code="404">If the car is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), 404)]
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

        /// <summary>
        /// Create a new car
        /// </summary>
        /// <param name="createCarDto">Car creation data</param>
        /// <returns>Created car</returns>
        /// <response code="201">Returns the newly created car</response>
        /// <response code="400">If the car data is invalid</response>
        /// <response code="401">If the user is not authenticated</response>
        /// <response code="403">If the user is not authorized</response>
        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<CarDto>), 400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
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