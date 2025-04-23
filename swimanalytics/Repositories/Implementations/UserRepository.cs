using Microsoft.EntityFrameworkCore;
using swimanalytics.Data;
using swimanalytics.Models.Entities;
using swimanalytics.Repositories.Interfaces;

namespace swimanalytics.Repositories.Implementations
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext repositoryContext) : base(repositoryContext) { }

        public ICollection<User> GetAll()
        {
            return FindAll()
                .ToList();
        }

        public User GetByEmail(string email) 
        {
            return FindByCondition(u => u.Email == email)
                .FirstOrDefault();
        }

        public User GetById(int id)
        {
            return FindByCondition(u => u.Id == id)
                .FirstOrDefault();
        }

        public void Remove(User user)
        {
            Delete(user);
            SaveChanges();
        }

        public void Save(User user)
        {
            if (user.Id == 0)
                Create(user);
            else
                Update(user);

            SaveChanges();
        }
    }
}
