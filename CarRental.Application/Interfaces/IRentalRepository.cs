using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface IRentalRepository : IGenericRepository<Rental>
    {
        Task<IEnumerable<Rental>> GetActiveRentalsAsync();
        Task<IEnumerable<Rental>> GetRentalsByCustomerAsync(int customerId);
        Task<IEnumerable<Rental>> GetRentalsByCarAsync(int carId);
        Task<IEnumerable<Rental>> GetOverdueRentalsAsync();
        Task<decimal> GetMonthlyRevenueAsync(int year, int month);
    }
}
