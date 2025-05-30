using QuizApp.Models.User;
using System.ComponentModel.DataAnnotations;
using QuizApp.Models.Questions;

namespace QuizApp.Models.Quiz;

public class QuizModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
    public int UserId { get; set; }
    public UserModel? User { get; set; }
    public ICollection<QuestionsModel> Questions { get; set; } = [];
}
