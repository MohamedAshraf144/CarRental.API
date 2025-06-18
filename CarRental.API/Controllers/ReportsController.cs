using CarRental.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.API.Controllers
{
    // ReportsController.cs
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Employee")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStatistics()
        {
            var response = await _reportService.GetDashboardStatisticsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("revenue/{year}")]
        public async Task<IActionResult> GetRevenueReport(int year)
        {
            var response = await _reportService.GetRevenueReportAsync(year);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("popular-cars")]
        public async Task<IActionResult> GetPopularCars([FromQuery] int topCount = 10)
        {
            var response = await _reportService.GetPopularCarsAsync(topCount);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
