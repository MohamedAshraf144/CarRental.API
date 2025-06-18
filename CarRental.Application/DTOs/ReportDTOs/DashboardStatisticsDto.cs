using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.ReportDTOs
{
    public class DashboardStatisticsDto
    {
        public int TotalCars { get; set; }
        public int AvailableCars { get; set; }
        public int ActiveRentals { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int NewCustomersThisMonth { get; set; }
        public List<RevenueByMonthDto> RevenueByMonth { get; set; }
        public List<PopularCarDto> PopularCars { get; set; }
    }

}
