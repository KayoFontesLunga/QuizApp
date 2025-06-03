using QuizApp.DTOs.Answer;

namespace QuizApp.DTOs.Question;

public class QuestionCreateDTO
{
    public string Text { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public List<AnswerCreateDTO> Answers { get; set; } = [];
}
