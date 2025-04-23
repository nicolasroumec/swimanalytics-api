using swimanalytics.Models.Entities;

namespace swimanalytics.Repositories.Interfaces
{
    public interface IUserRepository
    {
        ICollection<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        void Save(User user);
        void Remove(User user);
    }
}
