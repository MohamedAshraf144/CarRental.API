using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.RentalDTOs
{
    public class UpdateRentalDto
    {
        public int Id { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
    }
}
