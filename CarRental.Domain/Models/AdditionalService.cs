using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
    public class AdditionalService
    {
        public int Id { get; set; }
        public string Name { get; set; } // GPS, Child Seat, Insurance, etc.
        public decimal DailyRate { get; set; }
        public string Description { get; set; }
    }
}
