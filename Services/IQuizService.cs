using QuizApp.DTOs;
using QuizApp.DTOs.Quiz;
using QuizApp.DTOs.Submit;
using QuizApp.Models.Quiz;

namespace QuizApp.Services;

public interface IQuizService
{
    Task<QuizDTO> CreateQuizAsync(CreateQuizDTO createQuizDto, int userId);
    Task<QuizDTO?> UpdateQuizAsync(UpdateQuizDTO updateQuizDto, int userId);
    Task<bool> DeleteQuizAsync(int quizId, int userId);
    Task<List<QuizDTO>> GetAllQuizzesByUserAsync(int userId);
    Task<QuizDTO?> GetQuizByIdAsync(int quizId, int userId);
    Task<QuizResultDTO?> SubmitQuizAsync(SubmitQuizDTO submitQuizDto, int userId);
    Task<List<QuizRankingDTO>> GetRankingByQuizIdAsync(int quizId);
}
