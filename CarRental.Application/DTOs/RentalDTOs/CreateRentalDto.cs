using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.RentalDTOs
{
    public class CreateRentalDto
    {
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PickupLocation { get; set; }
        public string ReturnLocation { get; set; }
        public List<int> AdditionalServiceIds { get; set; }
        public decimal DepositAmount { get; set; }
    }
}
