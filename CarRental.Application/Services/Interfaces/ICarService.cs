using CarRental.Application.DTOs.CarDTOs;
using CarRental.Application.DTOs.CommonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Interfaces
{
    public interface ICarService
    {
        Task<ApiResponse<CarDto>> GetCarByIdAsync(int id);
        Task<ApiResponse<PagedResponse<CarDto>>> GetCarsAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<ApiResponse<IEnumerable<CarDto>>> GetAvailableCarsAsync(CarAvailabilityDto availability);
        Task<ApiResponse<CarDto>> CreateCarAsync(CreateCarDto createCarDto);
        Task<ApiResponse<CarDto>> UpdateCarAsync(UpdateCarDto updateCarDto);
        Task<ApiResponse<bool>> DeleteCarAsync(int id);
        Task<ApiResponse<bool>> CheckAvailabilityAsync(int carId, DateTime startDate, DateTime endDate);
    }
}
