namespace QuizApp.DTOs.Question;

public class QuestionDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int QuizId { get; set; }
}
