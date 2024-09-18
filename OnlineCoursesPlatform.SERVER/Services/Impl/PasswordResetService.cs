using Microsoft.Extensions.Caching.Memory;
using OnlineCoursesPlatform.SERVER.Repositories;

namespace OnlineCoursesPlatform.SERVER.Services.Impl
{
    public class PasswordResetService: IPasswordResetService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public PasswordResetService(IUserRepository userRepository, IEmailService emailService, IMemoryCache cache)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _cache = cache;
        }

        // Generar código OTP y enviarlo por correo
        public void GenerateAndSendOtp(string email)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // Generar código OTP
            var otpCode = GenerateOtpCode();

            // Almacenar temporalmente el OTP en la caché (válido por 10 minutos)
            _cache.Set($"otp_{email}", otpCode, TimeSpan.FromMinutes(10));

            // Enviar OTP por correo
            _emailService.SendOtpEmail(email, otpCode);
        }

        // Verificar el código OTP
        public bool VerifyOtp(string email, string otpCode)
        {
            if (_cache.TryGetValue($"otp_{email}", out string storedOtp))
            {
                if (storedOtp == otpCode)
                {
                    return true;
                }
            }
            return false;
        }

        // Generar código OTP de 6 dígitos
        public string GenerateOtpCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

    }
}
