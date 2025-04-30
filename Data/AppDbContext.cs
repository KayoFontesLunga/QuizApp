using Microsoft.EntityFrameworkCore;
using QuizApp.Mappings;
using QuizApp.Models.User;

namespace QuizApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
