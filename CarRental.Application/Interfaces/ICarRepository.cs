using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Interfaces
{
    public interface ICarRepository : IGenericRepository<Car>
    {
        Task<IEnumerable<Car>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Car>> GetCarsByCategoryAsync(int categoryId);
        Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate);
        Task<PagedResponse<Car>> GetPagedCarsAsync(int pageNumber, int pageSize, string searchTerm = null);
    }
}
