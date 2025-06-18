using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICarRepository Cars { get; }
        ICustomerRepository Customers { get; }
        IRentalRepository Rentals { get; }
        IPaymentRepository Payments { get; }
        IUserRepository Users { get; }
        IGenericRepository<CarCategory> CarCategories { get; }
        IGenericRepository<AdditionalService> AdditionalServices { get; }
        IGenericRepository<Maintenance> Maintenances { get; }
        Task<int> SaveAsync();
    }
}
