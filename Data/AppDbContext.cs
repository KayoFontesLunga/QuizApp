using Microsoft.EntityFrameworkCore;
using QuizApp.Mappings;
using QuizApp.Models.Answers;
using QuizApp.Models.Questions;
using QuizApp.Models.Quiz;
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
            modelBuilder.ApplyConfiguration(new QuizMap());
            modelBuilder.ApplyConfiguration(new AnswersMap());
            modelBuilder.ApplyConfiguration(new QuestionsMap());
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<QuizModel> Quizzes { get; set; }
        public DbSet<QuestionsModel> Questions { get; set; }
        public DbSet<AnswersModel> Answers { get; set; }
    }
}
