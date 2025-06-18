using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Domain.Models
{
    public class CarCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } // Economy, Luxury, SUV, etc.
        public string Description { get; set; }
        public List<Car> Cars { get; set; }
    }
}
