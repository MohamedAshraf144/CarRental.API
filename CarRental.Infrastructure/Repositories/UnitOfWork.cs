using CarRental.Application.Interfaces;
using CarRental.Domain.Models;
using CarRental.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CarRentalDbContext _context;
        private ICarRepository _cars;
        private ICustomerRepository _customers;
        private IRentalRepository _rentals;
        private IPaymentRepository _payments;
        private IUserRepository _users;
        private IGenericRepository<CarCategory> _carCategories;
        private IGenericRepository<AdditionalService> _additionalServices;
        private IGenericRepository<Maintenance> _maintenances;

        public UnitOfWork(CarRentalDbContext context)
        {
            _context = context;
        }

        public ICarRepository Cars => _cars ??= new CarRepository(_context);
        public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
        public IRentalRepository Rentals => _rentals ??= new RentalRepository(_context);
        public IPaymentRepository Payments => _payments ??= new PaymentRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IGenericRepository<CarCategory> CarCategories => _carCategories ??= new GenericRepository<CarCategory>(_context);
        public IGenericRepository<AdditionalService> AdditionalServices => _additionalServices ??= new GenericRepository<AdditionalService>(_context);
        public IGenericRepository<Maintenance> Maintenances => _maintenances ??= new GenericRepository<Maintenance>(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
