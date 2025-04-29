using swimanalytics.Models.Enums;

namespace swimanalytics.Models.DTOs
{
    public class UpdateProfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Wingspan { get; set; }
        public string Club { get; set; }
    }
}
