using QuizApp.Models.Questions;

namespace QuizApp.Models.Answers;

public class AnswersModel
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
    public QuestionsModel? Questions { get; set; }
}
