using QuizApp.Data;
using QuizApp.DTOs;
using QuizApp.Models.Quiz;

namespace QuizApp.Services;

public class QuizService : IQuizService
{
    private readonly AppDbContext _context;
    public QuizService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<QuizDTO> CreateQuizAsync(CreateQuizDTO createQuizDto, int userId)
    {
        var quiz = new QuizModel()
        {
            Title = createQuizDto.Title,
            Description = createQuizDto.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return new QuizDTO() 
        { 
            Id = quiz.Id, 
            Title = quiz.Title, 
            Description = quiz.Description,
            CreatedAt = quiz.CreatedAt 
        };
    }
}
