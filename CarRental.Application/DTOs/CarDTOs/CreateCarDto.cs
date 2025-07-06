using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.CarDTOs
{
    public class CreateCarDto
    {
        [Required(ErrorMessage = "Make is required")]
        [StringLength(50, ErrorMessage = "Make cannot exceed 50 characters")]
        public string Make { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1900, 2030, ErrorMessage = "Year must be between 1900 and 2030")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Plate number is required")]
        [StringLength(20, ErrorMessage = "Plate number cannot exceed 20 characters")]
        public string PlateNumber { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Daily rate is required")]
        [Range(0.01, 10000, ErrorMessage = "Daily rate must be between 0.01 and 10000")]
        public decimal DailyRate { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Fuel type is required")]
        public string FuelType { get; set; }

        [Required(ErrorMessage = "Transmission type is required")]
        public string Transmission { get; set; }

        [Required(ErrorMessage = "Number of seats is required")]
        [Range(1, 50, ErrorMessage = "Number of seats must be between 1 and 50")]
        public int NumberOfSeats { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
        public int Mileage { get; set; }
    }
}
