using CarRental.Application.DTOs.CommonDTOs;
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
    public class CarRepository : GenericRepository<Car>, ICarRepository
    {
        public CarRepository(CarRentalDbContext context) : base(context) { }

        public async Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        {
            var unavailableCarIds = await _context.Rentals
                .Where(r => r.Status != RentalStatus.Cancelled &&
                           ((r.StartDate <= startDate && r.EndDate >= startDate) ||
                            (r.StartDate <= endDate && r.EndDate >= endDate) ||
                            (r.StartDate >= startDate && r.EndDate <= endDate)))
                .Select(r => r.CarId)
                .Distinct()
                .ToListAsync();

            return await _context.Cars
                .Include(c => c.Category)
                .Where(c => c.IsAvailable && !unavailableCarIds.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetCarsByCategoryAsync(int categoryId)
        {
            return await _context.Cars
                .Include(c => c.Category)
                .Where(c => c.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate)
        {
            var car = await _context.Cars.FindAsync(carId);
            if (car == null || !car.IsAvailable) return false;

            var hasConflictingRental = await _context.Rentals
                .AnyAsync(r => r.CarId == carId &&
                              r.Status != RentalStatus.Cancelled &&
                              ((r.StartDate <= startDate && r.EndDate >= startDate) ||
                               (r.StartDate <= endDate && r.EndDate >= endDate) ||
                               (r.StartDate >= startDate && r.EndDate <= endDate)));

            return !hasConflictingRental;
        }

        public async Task<PagedResponse<Car>> GetPagedCarsAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var query = _context.Cars.Include(c => c.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Make.Contains(searchTerm) ||
                                        c.Model.Contains(searchTerm) ||
                                        c.PlateNumber.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var cars = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<Car>
            {
                Items = cars,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalCount = totalCount,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < totalPages
            };
        }
    }
}
