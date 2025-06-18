using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.ReportDTOs
{
    public class RevenueByMonthDto
    {
        public string Month { get; set; }
        public decimal Revenue { get; set; }
    }
}
