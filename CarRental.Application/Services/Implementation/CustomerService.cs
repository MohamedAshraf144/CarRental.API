using AutoMapper;
using CarRental.Application.DTOs.CommonDTOs;
using CarRental.Application.DTOs.CustomerDTOs;
using CarRental.Application.Interfaces;
using CarRental.Application.Services.Interfaces;
using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CustomerDto>> GetCustomerByIdAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return new ApiResponse<CustomerDto>
                    {
                        Success = false,
                        Message = "Customer not found"
                    };
                }

                var customerDto = _mapper.Map<CustomerDto>(customer);
                return new ApiResponse<CustomerDto>
                {
                    Success = true,
                    Data = customerDto
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerDto>
                {
                    Success = false,
                    Message = "An error occurred while retrieving the customer",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<CustomerDto>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _unitOfWork.Customers.GetAllAsync();
                var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

                return new ApiResponse<IEnumerable<CustomerDto>>
                {
                    Success = true,
                    Data = customerDtos
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<CustomerDto>>
                {
                    Success = false,
                    Message = "An error occurred while retrieving customers",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<CustomerDto>> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
        {
            try
            {
                // Check if email already exists
                var existingCustomer = await _unitOfWork.Customers.GetByEmailAsync(createCustomerDto.Email);
                if (existingCustomer != null)
                {
                    return new ApiResponse<CustomerDto>
                    {
                        Success = false,
                        Message = "A customer with this email already exists"
                    };
                }

                // Check if national ID already exists
                existingCustomer = await _unitOfWork.Customers.GetByNationalIdAsync(createCustomerDto.NationalId);
                if (existingCustomer != null)
                {
                    return new ApiResponse<CustomerDto>
                    {
                        Success = false,
                        Message = "A customer with this national ID already exists"
                    };
                }

                var customer = _mapper.Map<Customer>(createCustomerDto);
                customer.RegisteredDate = DateTime.UtcNow;
                customer.IsVerified = false;

                await _unitOfWork.Customers.AddAsync(customer);
                await _unitOfWork.SaveAsync();

                var customerDto = _mapper.Map<CustomerDto>(customer);

                return new ApiResponse<CustomerDto>
                {
                    Success = true,
                    Data = customerDto,
                    Message = "Customer created successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerDto>
                {
                    Success = false,
                    Message = "An error occurred while creating the customer",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<CustomerDto>> UpdateCustomerAsync(int id, CreateCustomerDto updateCustomerDto)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return new ApiResponse<CustomerDto>
                    {
                        Success = false,
                        Message = "Customer not found"
                    };
                }

                // Check if email is being changed and already exists
                if (customer.Email != updateCustomerDto.Email)
                {
                    var existingCustomer = await _unitOfWork.Customers.GetByEmailAsync(updateCustomerDto.Email);
                    if (existingCustomer != null && existingCustomer.Id != id)
                    {
                        return new ApiResponse<CustomerDto>
                        {
                            Success = false,
                            Message = "A customer with this email already exists"
                        };
                    }
                }

                // Update customer properties
                customer.FirstName = updateCustomerDto.FirstName;
                customer.LastName = updateCustomerDto.LastName;
                customer.Email = updateCustomerDto.Email;
                customer.Phone = updateCustomerDto.Phone;
                customer.Address = updateCustomerDto.Address;
                customer.City = updateCustomerDto.City;
                customer.Country = updateCustomerDto.Country;
                customer.DriverLicenseNumber = updateCustomerDto.DriverLicenseNumber;
                customer.DriverLicenseExpiry = updateCustomerDto.DriverLicenseExpiry;

                _unitOfWork.Customers.Update(customer);
                await _unitOfWork.SaveAsync();

                var customerDto = _mapper.Map<CustomerDto>(customer);

                return new ApiResponse<CustomerDto>
                {
                    Success = true,
                    Data = customerDto,
                    Message = "Customer updated successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<CustomerDto>
                {
                    Success = false,
                    Message = "An error occurred while updating the customer",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> VerifyCustomerAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.Customers.GetByIdAsync(id);
                if (customer == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "Customer not found"
                    };
                }

                customer.IsVerified = true;
                _unitOfWork.Customers.Update(customer);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Customer verified successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while verifying the customer",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
