namespace swimanalytics.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationEmail(string email, string code);
    }
}