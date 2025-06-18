using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.PaymentDTOs
{
    public class CreatePaymentDto
    {
        public int RentalId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; }
        public string Type { get; set; }
    }
}
