using CarRental.Application.Interfaces;
using CarRental.Domain.Enums;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Repositories
{
    public class RentalRepository : GenericRepository<Rental>, IRentalRepository
    {
        public RentalRepository(CarRentalDbContext context) : base(context) { }

        public async Task<IEnumerable<Rental>> GetActiveRentalsAsync()
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .Where(r => r.Status == RentalStatus.Active)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCustomerAsync(int customerId)
        {
            return await _context.Rentals
                .Include(r => r.Car)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetRentalsByCarAsync(int carId)
        {
            return await _context.Rentals
                .Include(r => r.Customer)
                .Where(r => r.CarId == carId)
                .OrderByDescending(r => r.StartDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rental>> GetOverdueRentalsAsync()
        {
            var currentDate = DateTime.UtcNow;
            return await _context.Rentals
                .Include(r => r.Car)
                .Include(r => r.Customer)
                .Where(r => r.Status == RentalStatus.Active && r.EndDate < currentDate)
                .ToListAsync();
        }

        public async Task<decimal> GetMonthlyRevenueAsync(int year, int month)
        {
            return await _context.Rentals
                .Where(r => r.StartDate.Year == year && r.StartDate.Month == month &&
                           r.Status != RentalStatus.Cancelled)
                .SumAsync(r => r.TotalAmount);
        }
    }
}
