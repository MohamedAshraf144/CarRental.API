using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.PaymentDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentDto>> ProcessPaymentAsync(CreatePaymentDto paymentDto);
        Task<ApiResponse<IEnumerable<PaymentDto>>> GetPaymentsByRentalAsync(int rentalId);
        Task<ApiResponse<PaymentDto>> RefundPaymentAsync(int paymentId, decimal amount);
    }
}
