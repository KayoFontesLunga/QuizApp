using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.DTOs;
using QuizApp.DTOs.Paginated;
using QuizApp.DTOs.Quiz;
using QuizApp.DTOs.Submit;
using QuizApp.Models.Quiz;
using System.Linq;

namespace QuizApp.Services;

public class QuizService : IQuizService
{
    private readonly AppDbContext _context;
    public QuizService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<QuizDTO?> CreateQuizAsync(CreateQuizDTO createQuizDto, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }
        if (createQuizDto.IsGlobal && !user.Role.Equals("Admin"))
        {
            throw new Exception("Only admins can create global quizzes.");
        }
        var quiz = new QuizModel()
        {
            Title = createQuizDto.Title,
            Description = createQuizDto.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = userId,
            IsGlobal = createQuizDto.IsGlobal
        };

        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
        return new QuizDTO() 
        { 
            Id = quiz.Id, 
            Title = quiz.Title, 
            Description = quiz.Description,
            CreatedAt = quiz.CreatedAt,
            IsGlobal = quiz.IsGlobal
        };
    }
    public async Task<QuizDTO?> UpdateQuizAsync(UpdateQuizDTO updateQuizDto, int userId)
    {
        var quiz = await _context.Quizzes.FindAsync(updateQuizDto.Id);

        if (quiz == null || quiz.UserId != userId)
        {
            return null;
        }

        quiz.Title = updateQuizDto.Title;
        quiz.Description = updateQuizDto.Description;

        await _context.SaveChangesAsync();

        return new QuizDTO
        {
            Id = quiz.Id,
            Title = quiz.Title,
            Description = quiz.Description,
            CreatedAt = quiz.CreatedAt
        };
    }
    public async Task<bool> DeleteQuizAsync(int quizId, int userId)
    {
        var quiz = await _context.Quizzes.FindAsync(quizId);

        if (quiz == null || quiz.UserId != userId)
            return false;

        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<List<QuizDTO>> GetAllQuizzesByUserAsync(int userId)
    {
        return await _context.Quizzes
            .Where(q => q.UserId == userId)
            .Select(q => new QuizDTO
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                CreatedAt = q.CreatedAt
            })
            .ToListAsync();
    }
    public async Task<QuizDTO?> GetQuizByIdAsync(int quizId, int userId)
    {
        var quiz = await _context.Quizzes
            .Where(q => q.Id == quizId && q.UserId == userId)
            .Select(q => new QuizDTO
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                CreatedAt = q.CreatedAt
            })
            .FirstOrDefaultAsync();

        return quiz;
    }

    public async Task<QuizResultDTO?> SubmitQuizAsync(SubmitQuizDTO submitQuizDto, int userId)
    {
        var quiz = await _context.Quizzes
            .Include(q => q.Questions!)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == submitQuizDto.QuizId);

        if (quiz == null || (!quiz.IsGlobal && quiz.UserId != userId))
        {
            return null;
        }
        int total = quiz.Questions.Count;
        int correct = 0;

        foreach (var submitted in submitQuizDto.Answers)
        {
            var question = quiz.Questions.FirstOrDefault(q => q.Id == submitted.QuestionId);
            if (question == null)
            {
                continue;
            }
            var answer = question.Answers.FirstOrDefault(a => a.Id == submitted.SelectedAnswerId);
            if (answer != null && answer.IsCorrect)
            {
                correct++;
            }
        }
        var quizResult = new QuizResultModel
        {
            QuizId = submitQuizDto.QuizId,
            UserId = userId,
            Score = correct,
            SubmittedAt = DateTime.UtcNow
        };
        _context.QuizResults.Add(quizResult);
        await _context.SaveChangesAsync();
        return new QuizResultDTO()
        {
            TotalQuestions = total,
            CorrectAnswers = correct,
            ScorePercentage = total > 0 ? Math.Round((double)correct / total * 100, 2) : 0
        };
    }

    public async Task<List<QuizRankingDTO>> GetRankingByQuizIdAsync(int quizId)
    {
        return await _context.QuizResults
            .Where(qr => qr.QuizId == quizId)
            .OrderByDescending(qr => qr.Score)
            .ThenBy(qr => qr.SubmittedAt)
            .Select(qr => new QuizRankingDTO
            {
                UserId = qr.UserId,
                UserName = qr.User!.Name,
                Score = qr.Score,
                SubmittedAt = qr.SubmittedAt
            })
            .ToListAsync();
    }
    public async Task<PaginatedResult<QuizDTO>> GetPublicQuizzesPaginatedAsync(int page, int pageSize)
    {
        var query = _context.Quizzes
            .Where(q => q.IsGlobal)
            .OrderByDescending(q => q.CreatedAt);

        var totalItems = await query.CountAsync();

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(q => new QuizDTO
            {
                Id = q.Id,
                Title = q.Title,
                Description = q.Description,
                CreatedAt = q.CreatedAt,
                IsGlobal = q.IsGlobal
            })
            .ToListAsync();
        return new PaginatedResult<QuizDTO>
        {
            Items = items,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };
    }
}
