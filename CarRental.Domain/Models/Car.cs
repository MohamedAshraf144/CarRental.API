using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string PlateNumber { get; set; }
        public string Color { get; set; }
        public int CategoryId { get; set; }
        public CarCategory Category { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public int Mileage { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public int NumberOfSeats { get; set; }
        public List<Rental> Rentals { get; set; }
        public List<Maintenance> Maintenances { get; set; }
    }
}
