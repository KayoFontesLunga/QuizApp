using QuizApp.DTOs.Question;

namespace QuizApp.Services;

public interface IQuestionService
{
    Task<QuestionDTO?> CreateQuestionAsync(QuestionCreateDTO createQuestionDto, int userId);
    Task<List<QuestionDTO>?> GetAllQuestionsByQuizIdAsync(int quizId, int userId);
    Task<QuestionDTO?> GetQuestionByIdAsync(int questionId, int userId);
    Task<QuestionDTO?> UpdateQuestionAsync(UpdateQuestionDTO updateQuestionDto, int userId);
    Task<bool> DeleteQuestionAsync(int questionId, int userId);
 }
