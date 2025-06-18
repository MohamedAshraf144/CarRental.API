using CarRental.Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Infrastructure.Services
{
    

    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
            // Generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Combine salt and hash
            byte[] hashBytes = new byte[salt.Length + (256 / 8)];
            Array.Copy(salt, 0, hashBytes, 0, salt.Length);
            Array.Copy(Convert.FromBase64String(hashed), 0, hashBytes, salt.Length, 256 / 8);

            return Convert.ToBase64String(hashBytes);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            try
            {
                byte[] hashBytes = Convert.FromBase64String(hashedPassword);

                // Extract the salt (first 128 bits)
                byte[] salt = new byte[128 / 8];
                Array.Copy(hashBytes, 0, salt, 0, salt.Length);

                // Hash the provided password with the extracted salt
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));

                // Extract the hash from the stored password
                byte[] storedHash = new byte[256 / 8];
                Array.Copy(hashBytes, salt.Length, storedHash, 0, storedHash.Length);

                // Compare the computed hash with the stored hash
                return hashed == Convert.ToBase64String(storedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
