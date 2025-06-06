using QuizApp.Models.Enum;

namespace QuizApp.DTOs.User
{
    public class UserListDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }
        public SexType SexTypes { get; set; }
    }
}
