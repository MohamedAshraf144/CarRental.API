using CarRental.Application.DTOs.AuthDTOs;
using CarRental.Application.DTOs.CommonDTOs;
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
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly IPasswordService _passwordService;

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }

        public async Task<ApiResponse<TokenDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByUsernameAsync(loginDto.Username);
                if (user == null || !_passwordService.VerifyPassword(user.PasswordHash, loginDto.Password))
                {
                    return new ApiResponse<TokenDto>
                    {
                        Success = false,
                        Message = "Invalid username or password"
                    };
                }

                if (!user.IsActive)
                {
                    return new ApiResponse<TokenDto>
                    {
                        Success = false,
                        Message = "User account is inactive"
                    };
                }

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<TokenDto>
                {
                    Success = true,
                    Data = new TokenDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Expiry = DateTime.UtcNow.AddMinutes(60),
                        Role = user.Role
                    },
                    Message = "Login successful"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    Message = "An error occurred during login",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<TokenDto>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Check if username already exists
                var existingUser = await _unitOfWork.Users.GetByUsernameAsync(registerDto.Username);
                if (existingUser != null)
                {
                    return new ApiResponse<TokenDto>
                    {
                        Success = false,
                        Message = "Username already exists"
                    };
                }

                // Check if email already exists
                existingUser = await _unitOfWork.Users.GetByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new ApiResponse<TokenDto>
                    {
                        Success = false,
                        Message = "Email already exists"
                    };
                }

                var user = new User
                {
                    Username = registerDto.Username,
                    Email = registerDto.Email,
                    PasswordHash = _passwordService.HashPassword(registerDto.Password),
                    Role = registerDto.Role ?? "Customer",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveAsync();

                var accessToken = _jwtService.GenerateAccessToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<TokenDto>
                {
                    Success = true,
                    Data = new TokenDto
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Expiry = DateTime.UtcNow.AddMinutes(60),
                        Role = user.Role
                    },
                    Message = "Registration successful"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    Message = "An error occurred during registration",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<TokenDto>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByRefreshTokenAsync(refreshToken);
                if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                {
                    return new ApiResponse<TokenDto>
                    {
                        Success = false,
                        Message = "Invalid or expired refresh token"
                    };
                }

                var newAccessToken = _jwtService.GenerateAccessToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<TokenDto>
                {
                    Success = true,
                    Data = new TokenDto
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken,
                        Expiry = DateTime.UtcNow.AddMinutes(60),
                        Role = user.Role
                    },
                    Message = "Token refreshed successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TokenDto>
                {
                    Success = false,
                    Message = "An error occurred while refreshing token",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> RevokeTokenAsync(string username)
        {
            try
            {
                var user = await _unitOfWork.Users.GetByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "User not found"
                    };
                }

                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.SaveAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "Token revoked successfully"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "An error occurred while revoking token",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
