using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.CarDTOs
{
    public class CreateCarDto
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string PlateNumber { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public decimal DailyRate { get; set; }
        public string ImageUrl { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public int NumberOfSeats { get; set; }
        public int Mileage { get; set; }
    }
}
