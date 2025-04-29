using swimanalytics.Models.DTOs;
using swimanalytics.Models.Responses;

namespace swimanalytics.Services.Interfaces
{
    public interface IUserService
    {
        public Response ChangePassword(ChangePasswordDTO model);
        public Response ChangePhone(ChangePhoneDTO model, string email);
        public Response GetAll();
        public Response GetByEmail(string email);
        public Task<Response> Register(RegisterDTO model);
        public Task<Response> ResendVerificationCode(string email);
        public Response UpdateProfile(UpdateProfileDTO model, string email);
        Response VerifyAccount(string email, string code);
    }
}
