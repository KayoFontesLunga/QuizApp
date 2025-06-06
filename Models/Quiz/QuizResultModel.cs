using QuizApp.Models.User;

namespace QuizApp.Models.Quiz;

public class QuizResultModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserModel? User { get; set; }
    public int QuizId { get; set; }
    public QuizModel? Quiz { get; set; }
    public int Score { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
