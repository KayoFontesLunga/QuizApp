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
    public bool IsGlobal { get; set; } = false;
    public bool IsAdmin { get; set; }
 
    public int UserId { get; set; }
    public UserModel? User { get; set; }
    public ICollection<QuestionsModel> Questions { get; set; } = [];
    public ICollection<QuizResultModel> QuizResults { get; set; } = new List<QuizResultModel>();

}
