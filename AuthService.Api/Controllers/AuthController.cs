using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using CryptoHub.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _auth;
        public AuthController(IAuthRepository auth)
        {
            _auth = auth;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var user =await _auth.RegisterAsync(registerRequestDto);
            if (user ==null)
            {
                return NotFound();
            }
            var response = ApiResponse<RegisterResponseDto>.SuccessResponse(user, "User Register successfully");
            return Ok(response);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _auth.LoginAsync(loginRequestDto);
            if (user == null)
            { 
                return NotFound();  
            }

            var response = ApiResponse<LoginResponseDto>.SuccessResponse(user, "User Logged In successfully");
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Test()
        {
            return Ok("Testing");
        }
    }
}
