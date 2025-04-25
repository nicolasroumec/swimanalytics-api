using swimanalytics.Models.Entities;

namespace swimanalytics.Repositories.Interfaces
{
    public interface IVerificationCodeRepository
    {
        VerificationCode GetByUserIdAndCode(int userId, string code);
        void Save(VerificationCode code);
    }
}
