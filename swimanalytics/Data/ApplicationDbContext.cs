using Microsoft.EntityFrameworkCore;
using swimanalytics.Models.Entities;

namespace swimanalytics.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<VerificationCode> VerificationCodes { get; set; }

    }
}