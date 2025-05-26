using QuizApp.DTOs;
using QuizApp.Models.Quiz;

namespace QuizApp.Services;

public interface IQuizService
{
    Task<QuizDTO> CreateQuizAsync(CreateQuizDTO createQuizDto, int userId);
}
