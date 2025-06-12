namespace swimanalytics.Models.DTOs
{
    public class ChangePasswordDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ResetToken { get; set; }
    }
}