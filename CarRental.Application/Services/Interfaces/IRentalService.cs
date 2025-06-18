using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.RentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Interfaces
{
    public interface IRentalService
    {
        Task<ApiResponse<RentalDto>> GetRentalByIdAsync(int id);
        Task<ApiResponse<IEnumerable<RentalDto>>> GetActiveRentalsAsync();
        Task<ApiResponse<IEnumerable<RentalDto>>> GetRentalsByCustomerAsync(int customerId);
        Task<ApiResponse<RentalDto>> CreateRentalAsync(CreateRentalDto createRentalDto);
        Task<ApiResponse<RentalDto>> UpdateRentalAsync(UpdateRentalDto updateRentalDto);
        Task<ApiResponse<bool>> CancelRentalAsync(int id);
        Task<ApiResponse<RentalDto>> CompleteRentalAsync(int id, DateTime returnDate);
        Task<ApiResponse<decimal>> CalculateRentalCostAsync(int carId, DateTime startDate, DateTime endDate, List<int> additionalServiceIds);
    }
}
