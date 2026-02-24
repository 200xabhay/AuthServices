using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Models;
using AuthService.Infrastructure.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

       

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == loginRequestDto.Email);

            if (user == null)
                throw new Exception("Invalid Email or Password");

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                loginRequestDto.Password, user.PasswordHash);

            if (!isPasswordValid)
                throw new Exception("Invalid Email or Password");

            var role = _context.Roles.FirstOrDefault(q => q.RoleId ==user.RoleId);
            user.Role.RoleName = role.RoleName;
            // Generate JWT Token
            var token = GenerateToken(user);

            return new LoginResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Token = token
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var user = new User
            {
                Name = registerRequestDto.Name,
                Email = registerRequestDto.Email,
                Gender = registerRequestDto.Gender,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequestDto.Password),
                RoleId = registerRequestDto.RoleId,

            };


            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return new RegisterResponseDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email
            };
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                 new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                  new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                  new Claim(ClaimTypes.Role , user.Role.RoleName),
                 new Claim(ClaimTypes.Email, user.Email),
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users =await _context.Users.ToListAsync();
            
            return users;
        }
    }
}
