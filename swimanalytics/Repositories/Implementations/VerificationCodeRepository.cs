using swimanalytics.Data;
using swimanalytics.Models.Entities;
using swimanalytics.Repositories.Interfaces;

namespace swimanalytics.Repositories.Implementations
{
    public class VerificationCodeRepository : BaseRepository<VerificationCode>, IVerificationCodeRepository
    {
        public VerificationCodeRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
        {
        }

        public VerificationCode GetByUserIdAndCode(int userId, string code)
        {
            return FindByCondition(v => v.UserId == userId && v.Code == code && !v.IsUsed)
                .FirstOrDefault() ?? new VerificationCode();
        }

        public void Save(VerificationCode code)
        {
            if (code.Id == 0)
                Create(code);
            else
                Update(code);

            SaveChanges();
        }
    }
}
