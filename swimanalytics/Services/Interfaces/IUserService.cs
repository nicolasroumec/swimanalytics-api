using swimanalytics.Models.DTOs;
using testpush.Models.Responses;

namespace swimanalytics.Services.Interfaces
{
    public interface IUserService
    {
        public Response GetAll();
        public Response GetByEmail(string email);
        public Task<Response> Register(RegisterDTO model);
        public Task<Response> ResendVerificationCode(string email);
        Response VerifyAccount(string email, string code);

    }
}
