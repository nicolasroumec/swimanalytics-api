using swimanalytics.Services.Interfaces;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace swimanalytics.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, IWebHostEnvironment environment, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _environment = environment;
            _logger = logger;
        }

        public async Task SendVerificationEmail(string email, string code)
        {
            try
            {
                var htmlBody = await GetEmailTemplate("EmailVerification.html", new Dictionary<string, string>
                {
                    { "{{CODE}}", code },
                    { "{{EMAIL}}", email },
                    { "{{YEAR}}", DateTime.Now.Year.ToString() }
                });

                await SendEmailAsync(email, "🏊‍♂️ Verifica tu cuenta - SwimAnalytics", htmlBody);
                _logger.LogInformation("Verification email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send verification email to {Email}", email);
                throw;
            }
        }

        public async Task SendPasswordResetEmail(string email, string resetToken)
        {
            try
            {
                var resetUrl = $"{_configuration["Frontend:BaseUrl"]}/reset-password?token={resetToken}&email={email}";

                var htmlBody = await GetEmailTemplate("PasswordReset.html", new Dictionary<string, string>
                {
                    { "{{RESET_URL}}", resetUrl },
                    { "{{EMAIL}}", email },
                    { "{{YEAR}}", DateTime.Now.Year.ToString() }
                });

                await SendEmailAsync(email, "🔒 Restablecer contraseña - SwimAnalytics", htmlBody);
                _logger.LogInformation("Password reset email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send password reset email to {Email}", email);
                throw;
            }
        }

        private async Task<string> GetEmailTemplate(string templateName, Dictionary<string, string> replacements)
        {
            try
            {
                var templatePath = Path.Combine(_environment.ContentRootPath, "Templates", "Email", templateName);

                _logger.LogDebug("Looking for email template at: {TemplatePath}", templatePath);

                if (!File.Exists(templatePath))
                {
                    _logger.LogError("Email template not found at path: {TemplatePath}", templatePath);
                    throw new FileNotFoundException($"Email template '{templateName}' not found at path: {templatePath}");
                }

                var template = await File.ReadAllTextAsync(templatePath, Encoding.UTF8);

                foreach (var replacement in replacements)
                {
                    template = template.Replace(replacement.Key, replacement.Value);
                }

                return template;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading or processing email template {TemplateName}", templateName);
                throw;
            }
        }

        private async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            try
            {
                var smtpServer = _configuration["Email:SmtpServer"];
                var smtpPortString = _configuration["Email:SmtpPort"];
                var fromEmail = _configuration["Email:FromEmail"];
                var username = _configuration["Email:Username"];
                var password = _configuration["Email:Password"];

                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(fromEmail))
                {
                    throw new InvalidOperationException("Email configuration is incomplete. Check SmtpServer and FromEmail settings.");
                }

                if (!int.TryParse(smtpPortString, out int smtpPort))
                {
                    _logger.LogWarning("Invalid SMTP port configuration, using default port 587");
                    smtpPort = 587;
                }

                _logger.LogDebug("Preparing to send email to {Email} via {SmtpServer}:{SmtpPort}", email, smtpServer, smtpPort);

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, "SwimAnalytics"),
                    Subject = subject,
                    IsBodyHtml = true
                };

                var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);

                var logoPath = Path.Combine(_environment.ContentRootPath, "Media", "logo.png");
                if (File.Exists(logoPath))
                {
                    var logo = new LinkedResource(logoPath, MediaTypeNames.Image.Png)
                    {
                        ContentId = "logo"
                    };
                    htmlView.LinkedResources.Add(logo);
                    _logger.LogDebug("Logo embedded from path: {LogoPath}", logoPath);
                }
                else
                {
                    _logger.LogWarning("Logo not found at path: {LogoPath}", logoPath);
                }

                message.AlternateViews.Add(htmlView);
                message.To.Add(email);

                using var client = new SmtpClient(smtpServer, smtpPort);

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new System.Net.NetworkCredential(username, password);
                    _logger.LogDebug("Using custom credentials for SMTP authentication");
                }
                else
                {
                    client.UseDefaultCredentials = true;
                    _logger.LogDebug("Using default credentials for SMTP authentication");
                }

                client.EnableSsl = smtpPort == 587 || smtpPort == 465;

                _logger.LogDebug("SMTP Configuration - Server: {Server}, Port: {Port}, SSL: {SSL}",
                    smtpServer, smtpPort, client.EnableSsl);

                await client.SendMailAsync(message);
                _logger.LogInformation("Email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}. Subject: {Subject}", email, subject);
                throw;
            }
        }
    }
}