using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetByNationalIdAsync(string nationalId);
        Task<IEnumerable<Customer>> GetVerifiedCustomersAsync();
    }
}
