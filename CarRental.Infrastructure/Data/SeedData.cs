using CarRental.Domain.Models;
using CarRental.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task SeedDatabaseAsync(CarRentalDbContext context)
        {
            // Seed Cars if empty
            if (!await context.Cars.AnyAsync())
            {
                var cars = new List<Car>
                {
                    new Car
                    {
                        Make = "Toyota",
                        Model = "Corolla",
                        Year = 2023,
                        PlateNumber = "ABC123",
                        Color = "White",
                        CategoryId = 1, // Economy
                        DailyRate = 45.00m,
                        IsAvailable = true,
                        ImageUrl = "https://example.com/corolla.jpg",
                        Mileage = 15000,
                        FuelType = "Petrol",
                        Transmission = "Automatic",
                        NumberOfSeats = 5
                    },
                    new Car
                    {
                        Make = "Mercedes-Benz",
                        Model = "E-Class",
                        Year = 2023,
                        PlateNumber = "LUX456",
                        Color = "Black",
                        CategoryId = 2, // Luxury
                        DailyRate = 150.00m,
                        IsAvailable = true,
                        ImageUrl = "https://example.com/eclass.jpg",
                        Mileage = 8000,
                        FuelType = "Petrol",
                        Transmission = "Automatic",
                        NumberOfSeats = 5
                    },
                    new Car
                    {
                        Make = "Nissan",
                        Model = "X-Trail",
                        Year = 2022,
                        PlateNumber = "SUV789",
                        Color = "Silver",
                        CategoryId = 3, // SUV
                        DailyRate = 85.00m,
                        IsAvailable = true,
                        ImageUrl = "https://example.com/xtrail.jpg",
                        Mileage = 20000,
                        FuelType = "Petrol",
                        Transmission = "Automatic",
                        NumberOfSeats = 7
                    },
                    new Car
                    {
                        Make = "Porsche",
                        Model = "911",
                        Year = 2023,
                        PlateNumber = "SPT001",
                        Color = "Red",
                        CategoryId = 4, // Sports
                        DailyRate = 300.00m,
                        IsAvailable = true,
                        ImageUrl = "https://example.com/911.jpg",
                        Mileage = 5000,
                        FuelType = "Petrol",
                        Transmission = "Manual",
                        NumberOfSeats = 2
                    }
                };

                await context.Cars.AddRangeAsync(cars);
            }

            // Seed Admin User if not exists
            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var passwordService = new PasswordService();
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@carrental.com",
                    PasswordHash = passwordService.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await context.Users.AddAsync(adminUser);
            }

            // Seed Sample Customer
            if (!await context.Customers.AnyAsync())
            {
                var customer = new Customer
                {
                    FirstName = "أحمد",
                    LastName = "محمد",
                    Email = "ahmed@example.com",
                    Phone = "01012345678",
                    NationalId = "12345678901234",
                    DriverLicenseNumber = "DL123456",
                    DriverLicenseExpiry = DateTime.UtcNow.AddYears(2),
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Address = "123 شارع التحرير",
                    City = "القاهرة",
                    Country = "مصر",
                    RegisteredDate = DateTime.UtcNow,
                    IsVerified = true
                };

                await context.Customers.AddAsync(customer);
            }

            await context.SaveChangesAsync();
        }
    }
}
