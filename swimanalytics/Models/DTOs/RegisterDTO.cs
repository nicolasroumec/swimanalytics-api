using swimanalytics.Models.Enums;

namespace swimanalytics.Models.DTOs
{
    public class RegisterDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Wingspan { get; set; }
        public string Club { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public UserRole role { get; set; }
    }
}