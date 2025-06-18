using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.ReportDTOs
{
    public class PopularCarDto
    {
        public string CarName { get; set; }
        public int RentalCount { get; set; }
    }
}
