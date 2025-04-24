using swimanalytics.Models.DTOs;
using swimanalytics.Models.Entities;
using swimanalytics.Models.Enums;
using testpush.Models.Responses;

namespace swimanalytics.Services.Interfaces
{
    public interface IAuthService
    {
        public Response Login(LoginDTO model, User user);
        public string MakeToken(string email, UserRole role, int minutes);
    }
}
