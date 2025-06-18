using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CarRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Infrastructure.Data
{

    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions<CarRentalDbContext> options) : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarCategory> CarCategories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<AdditionalService> AdditionalServices { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Car Configuration
            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Make).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PlateNumber).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.PlateNumber).IsUnique();
                entity.Property(e => e.DailyRate).HasPrecision(10, 2);

                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Cars)
                    .HasForeignKey(e => e.CategoryId);
            });

            // Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.NationalId).IsRequired().HasMaxLength(20);
                entity.HasIndex(e => e.NationalId).IsUnique();
                entity.Property(e => e.DriverLicenseNumber).IsRequired().HasMaxLength(20);
            });

            // Rental Configuration
            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
                entity.Property(e => e.DepositAmount).HasPrecision(10, 2);

                entity.HasOne(e => e.Car)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(e => e.CarId);

                entity.HasOne(e => e.Customer)
                    .WithMany(c => c.Rentals)
                    .HasForeignKey(e => e.CustomerId);
            });

            // Payment Configuration
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(10, 2);

                entity.HasOne(e => e.Rental)
                    .WithMany(r => r.Payments)
                    .HasForeignKey(e => e.RentalId);
            });

            // Maintenance Configuration
            modelBuilder.Entity<Maintenance>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Cost).HasPrecision(10, 2);

                entity.HasOne(e => e.Car)
                    .WithMany(c => c.Maintenances)
                    .HasForeignKey(e => e.CarId);
            });

            // AdditionalService Configuration
            modelBuilder.Entity<AdditionalService>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.DailyRate).HasPrecision(10, 2);
            });

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Many-to-Many relationship between Rental and AdditionalService
            modelBuilder.Entity<Rental>()
                .HasMany(r => r.AdditionalServices)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "RentalAdditionalService",
                    j => j.HasOne<AdditionalService>().WithMany().HasForeignKey("AdditionalServiceId"),
                    j => j.HasOne<Rental>().WithMany().HasForeignKey("RentalId"));

            // Seed Data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Car Categories
            modelBuilder.Entity<CarCategory>().HasData(
                new CarCategory { Id = 1, Name = "Economy", Description = "Affordable and fuel-efficient cars" },
                new CarCategory { Id = 2, Name = "Luxury", Description = "Premium vehicles with luxury features" },
                new CarCategory { Id = 3, Name = "SUV", Description = "Spacious sport utility vehicles" },
                new CarCategory { Id = 4, Name = "Sports", Description = "High-performance sports cars" }
            );

            // Seed Additional Services
            modelBuilder.Entity<AdditionalService>().HasData(
                new AdditionalService { Id = 1, Name = "GPS Navigation", DailyRate = 10, Description = "GPS navigation system" },
                new AdditionalService { Id = 2, Name = "Child Seat", DailyRate = 15, Description = "Child safety seat" },
                new AdditionalService { Id = 3, Name = "Additional Insurance", DailyRate = 25, Description = "Comprehensive insurance coverage" },
                new AdditionalService { Id = 4, Name = "Additional Driver", DailyRate = 20, Description = "Add another authorized driver" }
            );
        }
    }
}
