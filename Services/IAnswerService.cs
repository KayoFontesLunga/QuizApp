using QuizApp.DTOs.Answer;

namespace QuizApp.Services;

public interface IAnswerService
{
    Task<AnswerDTO?> CreateAnswerAsync(AnswerCreateDTO answerCreateDTO, int userId);
    Task<List<AnswerDTO>?> GetAllAnswersByQuestionIdAsync(int questionId, int userId);
}
