using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.CarDTOs
{
    public class UpdateCarDto
    {
        public int Id { get; set; }
        public decimal? DailyRate { get; set; }
        public bool? IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public int? Mileage { get; set; }
    }
}
