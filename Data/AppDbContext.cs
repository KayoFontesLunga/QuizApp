using Microsoft.EntityFrameworkCore;

namespace QuizApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
