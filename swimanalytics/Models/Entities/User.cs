using swimanalytics.Models.Enums;

namespace swimanalytics.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public float Wingspan { get; set; }
        public string Club {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public byte[] Hash { get; set; }
        public byte[] Salt { get; set; }
        public bool IsActive { get; set; }
        public UserRole Role { get; set; }
    }
}
