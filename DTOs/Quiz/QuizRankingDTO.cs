namespace QuizApp.DTOs.Quiz;

public class QuizRankingDTO
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int Score { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}
