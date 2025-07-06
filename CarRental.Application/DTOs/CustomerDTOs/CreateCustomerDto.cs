using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.CustomerDTOs
{
    public class CreateCustomerDto
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "National ID is required")]
        [StringLength(20, MinimumLength = 10, ErrorMessage = "National ID must be between 10 and 20 characters")]
        public string NationalId { get; set; }

        [Required(ErrorMessage = "Driver license number is required")]
        [StringLength(20, ErrorMessage = "Driver license number cannot exceed 20 characters")]
        public string DriverLicenseNumber { get; set; }

        [Required(ErrorMessage = "Driver license expiry date is required")]
        [DataType(DataType.Date)]
        public DateTime DriverLicenseExpiry { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        public string City { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }
    }
}
