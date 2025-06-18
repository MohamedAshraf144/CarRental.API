using AutoMapper;
using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.PaymentDTOs;
using CarRental.Application.Interfaces;
using CarRental.Application.Services.Interfaces;
using CarRental.Domain.Enums;
using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaymentDto>> ProcessPaymentAsync(CreatePaymentDto paymentDto)
        {
            try
            {
                var rental = await _unitOfWork.Rentals.GetByIdAsync(paymentDto.RentalId);
                if (rental == null)
                {
                    return new ApiResponse<PaymentDto>
                    {
                        Success = false,
                        Message = "Rental not found"
                    };
                }

                var payment = new Payment
                {
                    RentalId = paymentDto.RentalId,
                    Amount = paymentDto.Amount,
                    PaymentDate = DateTime.UtcNow,
                    Method = Enum.Parse<PaymentMethod>(paymentDto.Method),
                    Type = Enum.Parse<PaymentType>(paymentDto.Type),
                    Status = PaymentStatus.Pending,
                    TransactionId = Guid.NewGuid().ToString()
                };

                // Simulate payment processing
                // In a real application, integrate with payment gateway here
                payment.Status = PaymentStatus.Completed;

                // Update rental status if it's a deposit payment
                if (payment.Type == PaymentType.Deposit && rental.Status == RentalStatus.Reserved)
                {
                    rental.Status = RentalStatus.Active;
                    _unitOfWork.Rentals.Update(rental);
                }

                await _unitOfWork.Payments.AddAsync(payment);
                await _unitOfWork.SaveAsync();

                var paymentDtoResult = _mapper.Map<PaymentDto>(payment);

                return new ApiResponse<PaymentDto>
                {
                    Success = true,
                    Data = paymentDtoResult,
                    Message = "Payment processed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaymentDto>
                {
                    Success = false,
                    Message = "An error occurred while processing the payment",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<PaymentDto>>> GetPaymentsByRentalAsync(int rentalId)
        {
            try
            {
                var payments = await _unitOfWork.Payments.GetPaymentsByRentalAsync(rentalId);
                var paymentDtos = _mapper.Map<IEnumerable<PaymentDto>>(payments);

                return new ApiResponse<IEnumerable<PaymentDto>>
                {
                    Success = true,
                    Data = paymentDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<PaymentDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving payments",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<PaymentDto>> RefundPaymentAsync(int paymentId, decimal amount)
        {
            try
            {
                var originalPayment = await _unitOfWork.Payments.GetByIdAsync(paymentId);
                if (originalPayment == null)
                {
                    return new ApiResponse<PaymentDto>
                    {
                        Success = false,
                        Message = "Payment not found"
                    };
                }

                if (originalPayment.Status != PaymentStatus.Completed)
                {
                    return new ApiResponse<PaymentDto>
                    {
                        Success = false,
                        Message = "Only completed payments can be refunded"
                    };
                }

                if (amount > originalPayment.Amount)
                {
                    return new ApiResponse<PaymentDto>
                    {
                        Success = false,
                        Message = "Refund amount cannot exceed original payment amount"
                    };
                }

                var refundPayment = new Payment
                {
                    RentalId = originalPayment.RentalId,
                    Amount = -amount, // Negative amount for refund
                    PaymentDate = DateTime.UtcNow,
                    Method = originalPayment.Method,
                    Type = PaymentType.Refund,
                    Status = PaymentStatus.Completed,
                    TransactionId = Guid.NewGuid().ToString()
                };

                await _unitOfWork.Payments.AddAsync(refundPayment);
                await _unitOfWork.SaveAsync();

                var paymentDto = _mapper.Map<PaymentDto>(refundPayment);

                return new ApiResponse<PaymentDto>
                {
                    Success = true,
                    Data = paymentDto,
                    Message = "Refund processed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PaymentDto>
                {
                    Success = false,
                    Message = "An error occurred while processing the refund",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
