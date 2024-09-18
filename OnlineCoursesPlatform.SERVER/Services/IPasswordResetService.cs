namespace OnlineCoursesPlatform.SERVER.Services
{
    public interface IPasswordResetService
    {
        void GenerateAndSendOtp(string email);
        bool VerifyOtp(string email, string otpCode);
        string GenerateOtpCode();
    }
}
