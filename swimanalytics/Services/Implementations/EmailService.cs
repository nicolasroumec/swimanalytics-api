using System.Net.Mail;

namespace swimanalytics.Services.Implementations
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendVerificationEmail(string email, string code)
        {
            var smtpServer = _configuration["Email:SmtpServer"];
            var smtpPort = int.Parse(_configuration["Email:SmtpPort"]);
            var fromEmail = _configuration["Email:FromEmail"];

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "Activate your account",
                Body = $"<p>Your verification code is: <strong>{code}</strong></p>",
                IsBodyHtml = true
            };
            message.To.Add(email);

            using (var client = new SmtpClient(smtpServer, smtpPort))
            {
                client.EnableSsl = false;
                client.UseDefaultCredentials = true;
                await client.SendMailAsync(message);
            }
        }
    }
}
