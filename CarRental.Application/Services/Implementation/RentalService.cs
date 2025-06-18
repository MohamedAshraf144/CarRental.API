using AutoMapper;
using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.RentalDTOs;
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
    public class RentalService : IRentalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RentalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<RentalDto>> CreateRentalAsync(CreateRentalDto createRentalDto)
        {
            try
            {
                // Check car availability
                var isAvailable = await _unitOfWork.Cars.IsCarAvailableAsync(
                    createRentalDto.CarId,
                    createRentalDto.StartDate,
                    createRentalDto.EndDate);

                if (!isAvailable)
                {
                    return new ApiResponse<RentalDto>
                    {
                        Success = false,
                        Message = "Car is not available for the selected dates"
                    };
                }

                // Get car details
                var car = await _unitOfWork.Cars.GetByIdAsync(createRentalDto.CarId);
                var days = (createRentalDto.EndDate - createRentalDto.StartDate).Days;
                var carCost = car.DailyRate * days;

                // Calculate additional services cost
                decimal additionalServicesCost = 0;
                if (createRentalDto.AdditionalServiceIds != null && createRentalDto.AdditionalServiceIds.Any())
                {
                    var services = await _unitOfWork.AdditionalServices.FindAsync(
                        s => createRentalDto.AdditionalServiceIds.Contains(s.Id));
                    additionalServicesCost = services.Sum(s => s.DailyRate) * days;
                }

                // Create rental
                var rental = new Rental
                {
                    CarId = createRentalDto.CarId,
                    CustomerId = createRentalDto.CustomerId,
                    StartDate = createRentalDto.StartDate,
                    EndDate = createRentalDto.EndDate,
                    TotalAmount = carCost + additionalServicesCost,
                    DepositAmount = createRentalDto.DepositAmount,
                    PickupLocation = createRentalDto.PickupLocation,
                    ReturnLocation = createRentalDto.ReturnLocation,
                    Status = RentalStatus.Reserved
                };

                await _unitOfWork.Rentals.AddAsync(rental);
                await _unitOfWork.SaveAsync();

                // Load related data for response
                rental = await _unitOfWork.Rentals.GetByIdAsync(rental.Id);
                var rentalDto = _mapper.Map<RentalDto>(rental);

                return new ApiResponse<RentalDto>
                {
                    Success = true,
                    Data = rentalDto,
                    Message = "Rental created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RentalDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the rental",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<RentalDto>> CompleteRentalAsync(int id, DateTime returnDate)
        {
            try
            {
                var rental = await _unitOfWork.Rentals.GetByIdAsync(id);
                if (rental == null)
                {
                    return new ApiResponse<RentalDto>
                    {
                        Success = false,
                        Message = "Rental not found"
                    };
                }

                rental.ActualReturnDate = returnDate;
                rental.Status = RentalStatus.Completed;

                // Calculate late fees if applicable
                if (returnDate > rental.EndDate)
                {
                    var lateDays = (returnDate - rental.EndDate).Days;
                    var car = await _unitOfWork.Cars.GetByIdAsync(rental.CarId);
                    var lateFee = car.DailyRate * lateDays * 1.5m; // 50% surcharge for late returns
                    rental.TotalAmount += lateFee;
                }

                _unitOfWork.Rentals.Update(rental);
                await _unitOfWork.SaveAsync();

                var rentalDto = _mapper.Map<RentalDto>(rental);

                return new ApiResponse<RentalDto>
                {
                    Success = true,
                    Data = rentalDto,
                    Message = "Rental completed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RentalDto>
                {
                    Success = false,
                    Message = "An error occurred while completing the rental",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // Implement other IRentalService methods...
        public async Task<ApiResponse<RentalDto>> GetRentalByIdAsync(int id)
        {
            try
            {
                var rental = await _unitOfWork.Rentals.GetByIdAsync(id);
                if (rental == null)
                {
                    return new ApiResponse<RentalDto>
                    {
                        Success = false,
                        Message = "Rental not found"
                    };
                }

                var rentalDto = _mapper.Map<RentalDto>(rental);
                return new ApiResponse<RentalDto>
                {
                    Success = true,
                    Data = rentalDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RentalDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<RentalDto>>> GetActiveRentalsAsync()
        {
            try
            {
                var rentals = await _unitOfWork.Rentals.GetActiveRentalsAsync();
                var rentalDtos = _mapper.Map<IEnumerable<RentalDto>>(rentals);

                return new ApiResponse<IEnumerable<RentalDto>>
                {
                    Success = true,
                    Data = rentalDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<RentalDto>>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<RentalDto>>> GetRentalsByCustomerAsync(int customerId)
        {
            try
            {
                var rentals = await _unitOfWork.Rentals.GetRentalsByCustomerAsync(customerId);
                var rentalDtos = _mapper.Map<IEnumerable<RentalDto>>(rentals);

                return new ApiResponse<IEnumerable<RentalDto>>
                {
                    Success = true,
                    Data = rentalDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<RentalDto>>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<RentalDto>> UpdateRentalAsync(UpdateRentalDto updateRentalDto)
        {
            try
            {
                var rental = await _unitOfWork.Rentals.GetByIdAsync(updateRentalDto.Id);
                if (rental == null)
                {
                    return new ApiResponse<RentalDto>
                    {
                        Success = false,
                        Message = "Rental not found"
                    };
                }

                if (updateRentalDto.ActualReturnDate.HasValue)
                    rental.ActualReturnDate = updateRentalDto.ActualReturnDate;

                if (!string.IsNullOrEmpty(updateRentalDto.Status))
                    rental.Status = Enum.Parse<RentalStatus>(updateRentalDto.Status);

                if (!string.IsNullOrEmpty(updateRentalDto.Notes))
                    rental.Notes = updateRentalDto.Notes;

                _unitOfWork.Rentals.Update(rental);
                await _unitOfWork.SaveAsync();

                var rentalDto = _mapper.Map<RentalDto>(rental);

                return new ApiResponse<RentalDto>
                {
                    Success = true,
                    Data = rentalDto,
                    Message = "Rental updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<RentalDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> CancelRentalAsync(int id)
        {
            try
            {
                var rental = await _unitOfWork.Rentals.GetByIdAsync(id);
                if (rental == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Rental not found"
                    };
                }

                if (rental.Status == RentalStatus.Active)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Cannot cancel an active rental"
                    };
                }

                rental.Status = RentalStatus.Cancelled;
                _unitOfWork.Rentals.Update(rental);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Rental cancelled successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<decimal>> CalculateRentalCostAsync(int carId, DateTime startDate, DateTime endDate, List<int> additionalServiceIds)
        {
            try
            {
                var car = await _unitOfWork.Cars.GetByIdAsync(carId);
                if (car == null)
                {
                    return new ApiResponse<decimal>
                    {
                        Success = false,
                        Message = "Car not found"
                    };
                }

                var days = (endDate - startDate).Days;
                var carCost = car.DailyRate * days;

                decimal additionalServicesCost = 0;
                if (additionalServiceIds != null && additionalServiceIds.Any())
                {
                    var services = await _unitOfWork.AdditionalServices.FindAsync(
                        s => additionalServiceIds.Contains(s.Id));
                    additionalServicesCost = services.Sum(s => s.DailyRate) * days;
                }

                var totalCost = carCost + additionalServicesCost;

                return new ApiResponse<decimal>
                {
                    Success = true,
                    Data = totalCost,
                    Message = $"Total cost for {days} days: {totalCost:C}"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<decimal>
                {
                    Success = false,
                    Message = "An error occurred",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }

}
