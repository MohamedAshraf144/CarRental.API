using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.DTOs.AdditionalServiceDTOs
{
    public class AdditionalServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DailyRate { get; set; }
        public string Description { get; set; }
    }
}
