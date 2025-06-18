using CarRental.Application.DTOs.AuthDTOs;
using CarRental.Application.DTOs.CommonDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<TokenDto>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<TokenDto>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponse<TokenDto>> RefreshTokenAsync(string refreshToken);
        Task<ApiResponse<bool>> RevokeTokenAsync(string username);
    }
}
