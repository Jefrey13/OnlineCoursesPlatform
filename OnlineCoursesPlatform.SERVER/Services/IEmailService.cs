namespace OnlineCoursesPlatform.SERVER.Services
{
    public interface IEmailService
    {
        void SendOtpEmail(string email, string otpCode);
    }
}
