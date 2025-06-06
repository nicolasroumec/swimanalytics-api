using swimanalytics.Services.Interfaces;
using System.Net.Mail;
using System.Net.Mime;

namespace swimanalytics.Services.Implementations
{
    public class EmailService : IEmailService
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

            var htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Verificacion de cuenta - SwimAnalytics</title>
    <style>
        @media screen and (max-width: 600px) {{
            .container {{
                width: 100% !important;
                max-width: 100% !important;
                margin: 0 !important;
            }}
            .content {{
                padding: 20px !important;
            }}
            .header {{
                padding: 30px 20px !important;
            }}
            .main-content {{
                padding: 30px 20px !important;
            }}
            .code-section {{
                padding: 0 20px 20px 20px !important;
            }}
            .code-container {{
                padding: 20px !important;
            }}
            .button-section {{
                padding: 0 20px 30px 20px !important;
            }}
            .info-section {{
                padding: 0 20px 30px 20px !important;
            }}
            .footer {{
                padding: 20px !important;
            }}
            .logo {{
                width: 60px !important;
                height: 60px !important;
            }}
            .header-icon {{
                width: 50px !important;
                height: 50px !important;
            }}
            .main-title {{
                font-size: 24px !important;
            }}
            .welcome-title {{
                font-size: 22px !important;
            }}
            .code-text {{
                font-size: 28px !important;
                letter-spacing: 4px !important;
            }}
            .button {{
                padding: 12px 24px !important;
                font-size: 14px !important;
            }}
        }}
        
        @media screen and (max-width: 480px) {{
            .code-text {{
                font-size: 24px !important;
                letter-spacing: 2px !important;
            }}
            .main-title {{
                font-size: 20px !important;
            }}
            .welcome-title {{
                font-size: 18px !important;
            }}
        }}
    </style>
</head>
<body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; background-color: #f0f8ff;"">
    <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""background-color: #f0f8ff; min-height: 100vh;"">
        <tr>
            <td align=""center"" style=""padding: 20px 10px;"">
                <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""600"" class=""container"" style=""max-width: 600px; width: 100%; background-color: #ffffff; border-radius: 12px; box-shadow: 0 8px 32px rgba(46, 125, 185, 0.15); overflow: hidden;"">
                    
                    <!-- Header -->
                    <tr>
                        <td class=""header"" style=""background: linear-gradient(135deg, #2E7DB9 0%, #4A90C2 50%, #6BB6FF 100%); padding: 40px 30px; text-align: center;"">
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                <tr>
                                    <td align=""center"">
                                        <img src=""cid:logo"" alt=""SwimAnalytics Logo"" class=""logo"" style=""width: 80px; height: 80px; border-radius: 50%; margin: 0 auto 20px; border: 3px solid rgba(255,255,255,0.3); display: block; background-color: rgba(255,255,255,0.1);"" />
                                        <h1 class=""main-title"" style=""margin: 0; font-family: Arial, sans-serif; font-size: 32px; font-weight: bold; color: #ffffff;"">SwimAnalytics</h1>
                                        <p style=""margin: 10px 0 0 0; font-family: Arial, sans-serif; font-size: 16px; color: rgba(255,255,255,0.9);"">Plataforma de Analisis de carreras</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Contenido principal -->
                    <tr>
                        <td class=""main-content"" style=""padding: 50px 40px; text-align: center;"">
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                <tr>
                                    <td align=""center"">
                                        <div class=""header-icon"" style=""width: 60px; height: 60px; background: linear-gradient(135deg, #4A90C2, #6BB6FF); border-radius: 50%; margin: 0 auto 30px;""></div>
                                        
                                        <h2 class=""welcome-title"" style=""margin: 0 0 20px 0; font-family: Arial, sans-serif; font-size: 28px; font-weight: bold; color: #1a365d;"">¡Bienvenido!</h2>
                                        
                                        <p style=""margin: 0 0 30px 0; font-family: Arial, sans-serif; font-size: 16px; line-height: 1.6; color: #4a5568;"">
                                            Estas a un paso de acceder a tu cuenta. Utiliza el siguiente codigo de verificacion para activar tu cuenta en SwimAnalytics.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Codigo de verificacion -->
                    <tr>
                        <td class=""code-section"" style=""padding: 0 40px 30px 40px; text-align: center;"">
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin: 0 auto; background: linear-gradient(135deg, #f7fafc 0%, #edf2f7 100%); border: 2px solid #4A90C2; border-radius: 16px; width: 100%; max-width: 400px;"">
                                <tr>
                                    <td class=""code-container"" align=""center"" style=""padding: 30px;"">
                                        <p style=""margin: 0 0 15px 0; font-family: Arial, sans-serif; font-size: 14px; color: #718096; font-weight: bold;"">Tu codigo de verificacion</p>
                                        <div class=""code-text"" style=""font-family: Courier, monospace; font-size: 36px; font-weight: bold; color: #2E7DB9; letter-spacing: 8px; word-break: break-all;"">
                                            {code}
                                        </div>
                                        <p style=""margin: 20px 0 0 0; font-family: Arial, sans-serif; font-size: 13px; color: #a0aec0;"">
                                            <strong>Expira en 15 minutos</strong>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Boton de accion -->
                    <tr>
                        <td class=""button-section"" style=""padding: 0 40px 40px 40px; text-align: center;"">
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" style=""margin: 0 auto;"">
                                <tr>
                                    <td style=""background: linear-gradient(135deg, #2E7DB9 0%, #4A90C2 100%); border-radius: 30px; padding: 2px;"">
                                        <a href=""#"" class=""button"" style=""display: inline-block; padding: 14px 32px; font-family: Arial, sans-serif; font-size: 16px; font-weight: bold; color: #ffffff; text-decoration: none; border-radius: 28px; background: linear-gradient(135deg, #2E7DB9 0%, #4A90C2 100%); min-width: 120px; text-align: center;"">
                                            Ir a SwimAnalytics
                                        </a>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Informacion adicional -->
                    <tr>
                        <td class=""info-section"" style=""padding: 0 40px 40px 40px; text-align: center;"">
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                <tr>
                                    <td align=""center"">
                                        <div style=""background: rgba(74, 144, 194, 0.05); border-left: 4px solid #4A90C2; padding: 20px; border-radius: 8px; margin: 20px 0; text-align: left;"">
                                            <p style=""margin: 0; font-family: Arial, sans-serif; font-size: 14px; color: #4a5568; line-height: 1.5;"">
                                                <strong>Consejo:</strong> Una vez verificada tu cuenta, podras acceder a analisis detallados de rendimiento, estadisticas personalizadas y seguimiento de tu progreso en natacion.
                                            </p>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                    <!-- Footer -->
                    <tr>
                        <td class=""footer"" style=""background: linear-gradient(135deg, #f8fafc 0%, #e2e8f0 100%); padding: 30px 40px; border-top: 1px solid #e2e8f0;"">
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                <tr>
                                    <td align=""center"">
                                        <p style=""margin: 0 0 10px 0; font-family: Arial, sans-serif; font-size: 13px; color: #718096; line-height: 1.5;"">
                                            Si no solicitaste esta verificacion, puedes ignorar este email de forma segura.
                                        </p>
                                        <div style=""height: 1px; background: linear-gradient(90deg, transparent 0%, #cbd5e0 50%, transparent 100%); margin: 20px 0;""></div>
                                        <p style=""margin: 0; font-family: Arial, sans-serif; font-size: 12px; color: #a0aec0;"">
                                            © 2024 SwimAnalytics - Todos los derechos reservados<br>
                                            <span style=""color: #4A90C2;"">Impulsando el rendimiento en natacion</span>
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

            var message = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = "🏊‍♂️ Verifica tu cuenta - SwimAnalytics",
                Body = htmlBody,
                IsBodyHtml = true
            };

            var htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);

            // Embeber el logo con el tipo MIME correcto
            var logoPath = Path.Combine("Media", "logo.png");
            if (File.Exists(logoPath))
            {
                var logo = new LinkedResource(logoPath, MediaTypeNames.Image.Png) // Cambiado a PNG
                {
                    ContentId = "logo" // Este debe coincidir con cid:logo en el HTML
                };
                htmlView.LinkedResources.Add(logo);
            }

            message.AlternateViews.Add(htmlView);
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