namespace QuizApp.DTOs.Answer;

public class AnswerCreateDTO
{
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
