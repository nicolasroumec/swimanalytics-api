namespace swimanalytics.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string email, string code);
        Task SendPasswordResetEmail(string email, string resetToken);
    }
}