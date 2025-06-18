using AutoMapper;
using CarRental.Application.DTOs.AdditionalServiceDTOs;
using CarRental.Application.DTOs.CarDTOs;
using CarRental.Application.DTOs.CustomerDTOs;
using CarRental.Application.DTOs.PaymentDTOs;
using CarRental.Application.DTOs.RentalDTOs;
using CarRental.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CarRental.Infrastructure.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Car Mappings
            CreateMap<Car, CarDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CreateCarDto, Car>();

            // Customer Mappings
            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.TotalRentals, opt => opt.MapFrom(src => src.Rentals.Count));
            CreateMap<CreateCustomerDto, Customer>();

            // Rental Mappings
            CreateMap<Rental, RentalDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreateRentalDto, Rental>();

            // Payment Mappings
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<CreatePaymentDto, Payment>();

            // Additional Service Mappings
            CreateMap<AdditionalService, AdditionalServiceDto>();
        }
    }
}
