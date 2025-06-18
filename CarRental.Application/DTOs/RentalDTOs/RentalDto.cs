using CarRental.Application.DTOs.AdditionalServiceDTOs;
using CarRental.Application.DTOs.CarDTOs;
using CarRental.Application.DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.RentalDTOs
{
    public class RentalDto
    {
        public int Id { get; set; }
        public CarDto Car { get; set; }
        public CustomerDto Customer { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnLocation { get; set; }
        public List<AdditionalServiceDto> AdditionalServices { get; set; }
    }
}
