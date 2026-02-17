using AuthService.Application.DTOs;
using AuthService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Application.Interfaces
{
    public interface IAuthRepository
    {

        Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);

        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);


        string GenerateToken(User user);

    }
}
