using AutoMapper;
using CarRental.Application.DTOs.CarDTOs;
using CarRental.Application.DTOs.CommonDTOs;
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
    public class CarService : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CarService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CarDto>> GetCarByIdAsync(int id)
        {
            try
            {
                var car = await _unitOfWork.Cars.GetByIdAsync(id);
                if (car == null)
                {
                    return new ApiResponse<CarDto>
                    {
                        Success = false,
                        Message = "Car not found"
                    };
                }

                var carDto = _mapper.Map<CarDto>(car);
                return new ApiResponse<CarDto>
                {
                    Success = true,
                    Data = carDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CarDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the car",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<PagedResponse<CarDto>>> GetCarsAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            try
            {
                var pagedCars = await _unitOfWork.Cars.GetPagedCarsAsync(pageNumber, pageSize, searchTerm);
                var carDtos = _mapper.Map<List<CarDto>>(pagedCars.Items);

                var response = new PagedResponse<CarDto>
                {
                    Items = carDtos,
                    PageNumber = pagedCars.PageNumber,
                    PageSize = pagedCars.PageSize,
                    TotalPages = pagedCars.TotalPages,
                    TotalCount = pagedCars.TotalCount,
                    HasPreviousPage = pagedCars.HasPreviousPage,
                    HasNextPage = pagedCars.HasNextPage
                };

                return new ApiResponse<PagedResponse<CarDto>>
                {
                    Success = true,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedResponse<CarDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving cars",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CarDto>>> GetAvailableCarsAsync(CarAvailabilityDto availability)
        {
            try
            {
                var availableCars = await _unitOfWork.Cars.GetAvailableCarsAsync(availability.StartDate, availability.EndDate);

                if (availability.CategoryId.HasValue)
                {
                    availableCars = availableCars.Where(c => c.CategoryId == availability.CategoryId.Value);
                }

                if (availability.MinPrice.HasValue)
                {
                    availableCars = availableCars.Where(c => c.DailyRate >= availability.MinPrice.Value);
                }

                if (availability.MaxPrice.HasValue)
                {
                    availableCars = availableCars.Where(c => c.DailyRate <= availability.MaxPrice.Value);
                }

                var carDtos = _mapper.Map<IEnumerable<CarDto>>(availableCars);

                return new ApiResponse<IEnumerable<CarDto>>
                {
                    Success = true,
                    Data = carDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CarDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving available cars",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<CarDto>> CreateCarAsync(CreateCarDto createCarDto)
        {
            try
            {
                var car = _mapper.Map<Car>(createCarDto);
                car.IsAvailable = true;

                await _unitOfWork.Cars.AddAsync(car);
                await _unitOfWork.SaveAsync();

                var carDto = _mapper.Map<CarDto>(car);

                return new ApiResponse<CarDto>
                {
                    Success = true,
                    Data = carDto,
                    Message = "Car created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CarDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the car",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<CarDto>> UpdateCarAsync(UpdateCarDto updateCarDto)
        {
            try
            {
                var car = await _unitOfWork.Cars.GetByIdAsync(updateCarDto.Id);
                if (car == null)
                {
                    return new ApiResponse<CarDto>
                    {
                        Success = false,
                        Message = "Car not found"
                    };
                }

                if (updateCarDto.DailyRate.HasValue)
                    car.DailyRate = updateCarDto.DailyRate.Value;

                if (updateCarDto.IsAvailable.HasValue)
                    car.IsAvailable = updateCarDto.IsAvailable.Value;

                if (!string.IsNullOrEmpty(updateCarDto.ImageUrl))
                    car.ImageUrl = updateCarDto.ImageUrl;

                if (updateCarDto.Mileage.HasValue)
                    car.Mileage = updateCarDto.Mileage.Value;

                _unitOfWork.Cars.Update(car);
                await _unitOfWork.SaveAsync();

                var carDto = _mapper.Map<CarDto>(car);

                return new ApiResponse<CarDto>
                {
                    Success = true,
                    Data = carDto,
                    Message = "Car updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CarDto>
                {
                    Success = false,
                    Message = "An error occurred while updating the car",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteCarAsync(int id)
        {
            try
            {
                var car = await _unitOfWork.Cars.GetByIdAsync(id);
                if (car == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Car not found"
                    };
                }

                // Check if car has active rentals
                var activeRentals = await _unitOfWork.Rentals.FindAsync(r => r.CarId == id && r.Status == RentalStatus.Active);
                if (activeRentals.Any())
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Cannot delete car with active rentals"
                    };
                }

                _unitOfWork.Cars.Remove(car);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Car deleted successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while deleting the car",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> CheckAvailabilityAsync(int carId, DateTime startDate, DateTime endDate)
        {
            try
            {
                var isAvailable = await _unitOfWork.Cars.IsCarAvailableAsync(carId, startDate, endDate);

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = isAvailable,
                    Message = isAvailable ? "Car is available" : "Car is not available for the selected dates"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while checking availability",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
