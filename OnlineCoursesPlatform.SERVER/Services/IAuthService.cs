using OnlineCoursesPlatform.DTO.RequestDTO;
using OnlineCoursesPlatform.SERVER.Models;

namespace OnlineCoursesPlatform.SERVER.Services
{
    public interface IAuthService
    {
        bool ValidateUser(LoginRequestDTO loginDto, out User user);
        string GenerateToken(User user);
        string HashPassword(string password);
        bool VerifyPassword(string inputPassword, string storedPassword);

        string GenerateRefreshToken(User user);
        bool ValidateRefreshToken(string refreshToken, out User user);

        bool Register(RegisterRequestDTO registerDto, out User user);
    }
}
