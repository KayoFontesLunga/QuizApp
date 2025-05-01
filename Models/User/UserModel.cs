using QuizApp.Models.Enum;

namespace QuizApp.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public SexType SexTypes { get; set; }

    }
}
