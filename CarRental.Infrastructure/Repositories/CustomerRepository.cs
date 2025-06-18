using CarRental.Application.Interfaces;
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
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(CarRentalDbContext context) : base(context) { }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer> GetByNationalIdAsync(string nationalId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.NationalId == nationalId);
        }

        public async Task<IEnumerable<Customer>> GetVerifiedCustomersAsync()
        {
            return await _context.Customers.Where(c => c.IsVerified).ToListAsync();
        }
    }

}
