using OnlineCoursesPlatform.DTO.RequestDTO;

namespace OnlineCoursesPlatform.CLIENT.Services
{
    public interface IAuthService
    {
        Task<HttpResponseMessage> Login(LoginRequestDTO loginRequest);
        Task<HttpResponseMessage> Register(RegisterRequestDTO registerRequest);
        Task<HttpResponseMessage> RefreshToken(RefreshTokenRequestDTO refreshTokenRequest);
        Task<HttpResponseMessage> RequestOtp(RequestOtpDTO otpRequest);
        Task<HttpResponseMessage> VerifyOtp(VerifyOtpRequestDTO otpVerifyRequest);
    }
}
