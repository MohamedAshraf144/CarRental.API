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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(CarRentalDbContext context) : base(context) { }

        public async Task<IEnumerable<Payment>> GetPaymentsByRentalAsync(int rentalId)
        {
            return await _context.Payments
                .Where(p => p.RentalId == rentalId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPaymentsByRentalAsync(int rentalId)
        {
            return await _context.Payments
                .Where(p => p.RentalId == rentalId && p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);
        }
    }
}
