using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace OnlineCoursesPlatform.SERVER.Services.Impl
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendOtpEmail(string email, string otpCode)
        {
            var message = new MimeMessage();

            // Los valores de remitente se obtienen del archivo .env
            var fromEmail = _config["SMTP_EMAIL"];
            var fromName = _config["SMTP_NAME"];

            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress("User", email));
            message.Subject = "Password Reset";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP code is: {otpCode}",
            };

            using (var client = new SmtpClient())
            {
                // Valores del archivo .env
                var smtpHost = _config["SMTP_HOST"];
                var smtpPort = int.Parse(_config["SMTP_PORT"]);
                var smtpEmail = _config["SMTP_EMAIL"];
                var smtpPassword = _config["SMTP_PASSWORD"];

                client.Connect(smtpHost, smtpPort, false);
                client.Authenticate(smtpEmail, smtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}