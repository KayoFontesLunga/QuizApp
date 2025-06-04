namespace QuizApp.DTOs.Answer;

public class AnswerUpdateDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
