using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCoursesPlatform.DTO.RequestDTO;
using OnlineCoursesPlatform.DTO.ResponseDTO;
using OnlineCoursesPlatform.SERVER.Models;
using OnlineCoursesPlatform.SERVER.Services;

namespace OnlineCoursesPlatform.SERVER.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IPasswordResetService _passwordResetService;

        public AuthController(IAuthService authService, IPasswordResetService passwordResetService)
        {
            _authService = authService;
            _passwordResetService = passwordResetService;
        }

        // POST: api/v1/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Email and password must be provided." });
            }

            // Validar las credenciales del usuario
            if(_authService.ValidateUser(loginRequest, out User user))
            {
                // Generar el token JWT con roles y permisos dinámicos
                var token = _authService.GenerateToken(user);

                var response = new UserResponseDTO
                {
                    Token = token,
                };

                return Ok(response);
            }
            return Unauthorized(new { message = "Invalid credentials." });
        }

        // POST: api/v1/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequestDTO registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_authService.Register(registerRequest, out User user))
            {
                return Ok(new { message = "User registered successfully" });
            }

            return BadRequest(new { message = "Email already in use" });
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenRequestDTO refreshTokenRequest)
        {
            if (string.IsNullOrEmpty(refreshTokenRequest.RefreshToken))
            {
                return BadRequest(new { message = "Refresh token must be provided." });
            }

            if (_authService.ValidateRefreshToken(refreshTokenRequest.RefreshToken, out User user))
            {
                // Generar nuevo token JWT y refresh token
                var newToken = _authService.GenerateToken(user);
                var newRefreshToken = _authService.GenerateRefreshToken(user);

                return Ok(new
                {
                    Token = newToken,
                    RefreshToken = newRefreshToken,
                    Expiration = DateTime.UtcNow.AddMinutes(30)
                });
            }

            return Unauthorized(new { message = "Invalid or expired refresh token." });
        }

        [HttpPost("request-otp")]
        public IActionResult RequestOtp([FromBody] RequestOtpDTO request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { message = "Email is required" });
            }

            try
            {
                _passwordResetService.GenerateAndSendOtp(request.Email);
                return Ok(new { message = "OTP sent to email" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpRequestDTO request)
        {
            if (_passwordResetService.VerifyOtp(request.Email, request.OtpCode))
            {
                return Ok(new { message = "OTP verified" });
            }

            return BadRequest(new { message = "Invalid OTP" });
        }
    }
}
