using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.ReportDTOs;
using CarRental.Application.Interfaces;
using CarRental.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Implementation
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<DashboardStatisticsDto>> GetDashboardStatisticsAsync()
        {
            try
            {
                var currentDate = DateTime.UtcNow;
                var startOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);

                var totalCars = await _unitOfWork.Cars.GetAllAsync();
                var availableCars = totalCars.Where(c => c.IsAvailable).Count();
                var activeRentals = await _unitOfWork.Rentals.GetActiveRentalsAsync();
                var monthlyRevenue = await _unitOfWork.Rentals.GetMonthlyRevenueAsync(currentDate.Year, currentDate.Month);

                var newCustomersThisMonth = await _unitOfWork.Customers.FindAsync(
                    c => c.RegisteredDate >= startOfMonth && c.RegisteredDate <= currentDate);

                // Get revenue by month for the last 6 months
                var revenueByMonth = new List<RevenueByMonthDto>();
                for (int i = 5; i >= 0; i--)
                {
                    var date = currentDate.AddMonths(-i);
                    var revenue = await _unitOfWork.Rentals.GetMonthlyRevenueAsync(date.Year, date.Month);
                    revenueByMonth.Add(new RevenueByMonthDto
                    {
                        Month = date.ToString("MMM yyyy"),
                        Revenue = revenue
                    });
                }

                // Get popular cars
                var rentals = await _unitOfWork.Rentals.GetAllAsync();
                var popularCars = rentals
                    .GroupBy(r => r.CarId)
                    .Select(g => new { CarId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToList();

                var popularCarDtos = new List<PopularCarDto>();
                foreach (var item in popularCars)
                {
                    var car = await _unitOfWork.Cars.GetByIdAsync(item.CarId);
                    popularCarDtos.Add(new PopularCarDto
                    {
                        CarName = $"{car.Make} {car.Model}",
                        RentalCount = item.Count
                    });
                }

                var statistics = new DashboardStatisticsDto
                {
                    TotalCars = totalCars.Count(),
                    AvailableCars = availableCars,
                    ActiveRentals = activeRentals.Count(),
                    MonthlyRevenue = monthlyRevenue,
                    NewCustomersThisMonth = newCustomersThisMonth.Count(),
                    RevenueByMonth = revenueByMonth,
                    PopularCars = popularCarDtos
                };

                return new ApiResponse<DashboardStatisticsDto>
                {
                    Success = true,
                    Data = statistics
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<DashboardStatisticsDto>
                {
                    Success = false,
                    Message = "An error occurred while generating statistics",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<RevenueByMonthDto>>> GetRevenueReportAsync(int year)
        {
            try
            {
                var revenueByMonth = new List<RevenueByMonthDto>();

                for (int month = 1; month <= 12; month++)
                {
                    var revenue = await _unitOfWork.Rentals.GetMonthlyRevenueAsync(year, month);
                    revenueByMonth.Add(new RevenueByMonthDto
                    {
                        Month = new DateTime(year, month, 1).ToString("MMMM"),
                        Revenue = revenue
                    });
                }

                return new ApiResponse<IEnumerable<RevenueByMonthDto>>
                {
                    Success = true,
                    Data = revenueByMonth
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<RevenueByMonthDto>>
                {
                    Success = false,
                    Message = "An error occurred while generating revenue report",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<PopularCarDto>>> GetPopularCarsAsync(int topCount = 10)
        {
            try
            {
                var rentals = await _unitOfWork.Rentals.GetAllAsync();
                var popularCars = rentals
                    .GroupBy(r => r.CarId)
                    .Select(g => new { CarId = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(topCount)
                    .ToList();

                var popularCarDtos = new List<PopularCarDto>();
                foreach (var item in popularCars)
                {
                    var car = await _unitOfWork.Cars.GetByIdAsync(item.CarId);
                    popularCarDtos.Add(new PopularCarDto
                    {
                        CarName = $"{car.Make} {car.Model} ({car.Year})",
                        RentalCount = item.Count
                    });
                }

                return new ApiResponse<IEnumerable<PopularCarDto>>
                {
                    Success = true,
                    Data = popularCarDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PopularCarDto>>
                {
                    Success = false,
                    Message = "An error occurred while getting popular cars",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
