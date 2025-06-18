using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.CustomerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<ApiResponse<CustomerDto>> GetCustomerByIdAsync(int id);
        Task<ApiResponse<IEnumerable<CustomerDto>>> GetAllCustomersAsync();
        Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
        Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(int id, CreateCustomerDto updateCustomerDto);
        Task<ApiResponse<bool>> VerifyCustomerAsync(int id);
    }
}
