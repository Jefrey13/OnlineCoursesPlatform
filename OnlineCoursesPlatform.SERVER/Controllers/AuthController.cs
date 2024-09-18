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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
    }
}
