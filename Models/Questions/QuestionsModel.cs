using QuizApp.Models.Answers;
using QuizApp.Models.Quiz;

namespace QuizApp.Models.Questions;

public class QuestionsModel
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public int QuizId { get; set; }
    public QuizModel? Quiz { get; set; }
    public ICollection<AnswersModel> Answers { get; set; } = [];
}
