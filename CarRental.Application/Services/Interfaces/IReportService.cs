using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.ReportDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<ApiResponse<DashboardStatisticsDto>> GetDashboardStatisticsAsync();
        Task<ApiResponse<IEnumerable<RevenueByMonthDto>>> GetRevenueReportAsync(int year);
        Task<ApiResponse<IEnumerable<PopularCarDto>>> GetPopularCarsAsync(int topCount = 10);
    }
}
