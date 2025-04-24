using swimanalytics.Models.DTOs;
using testpush.Models.Responses;

namespace swimanalytics.Services.Interfaces
{
    public interface IUserService
    {
        public Response GetAll();
        public Response GetByEmail(string email);
        public Response Register(RegisterDTO model);
    }
}
