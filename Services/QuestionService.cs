using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.DTOs.Question;
using QuizApp.Models.Answers;
using QuizApp.Models.Questions;

namespace QuizApp.Services;

public class QuestionService : IQuestionService
{
    private readonly AppDbContext _context;
    public QuestionService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<QuestionDTO?> CreateQuestionAsync(QuestionCreateDTO createQuestionDto, int userId)
    {
      var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == createQuestionDto.QuizId && q.UserId == userId);
      if (quiz == null || quiz.UserId != userId)
      {
          return null;
      }
      var question = new QuestionsModel
      {
          Text = createQuestionDto.Text,
          QuizId = createQuestionDto.QuizId,
          Answers = [.. createQuestionDto.Answers.Select(a => new AnswersModel 
          { 
              Text = a.Text,
              IsCorrect = a.IsCorrect 
          })]
      };
      _context.Questions.Add(question);
      await _context.SaveChangesAsync();
      return new QuestionDTO
      {
          Id = question.Id,
          Text = question.Text,
          QuizId = question.QuizId
      };
    }
    public async Task<bool> DeleteQuestionAsync(int questionId, int userId)
    {
        var question = await _context.Questions.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == questionId && q.Quiz!.UserId == userId);
        if (question == null || question.Quiz!.UserId != userId)
        {
            return false;
        }
        else
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public async Task<List<QuestionDTO>?> GetAllQuestionsByQuizIdAsync(int quizId, int userId)
    {
        var quiz = await _context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == quizId && q.UserId == userId);
        if (quiz == null || quiz.UserId != userId)
        {
            return null;
        }
        return await _context.Questions
            .Where(q => q.QuizId == quizId)
            .Select(q => new QuestionDTO
            {
                Id = q.Id,
                Text = q.Text,
                QuizId = q.QuizId
            })
            .ToListAsync();
    }
    public async Task<QuestionDTO?> GetQuestionByIdAsync(int questionId, int userId)
    {
        var question = await _context.Questions.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == questionId && q.Quiz!.UserId == userId);
        if (question == null || question.Quiz!.UserId != userId)
        {
            return null;
        }
        return await _context.Questions
            .Where(q => q.Id == questionId)
            .Select(q => new QuestionDTO
            {
                Id = q.Id,
                Text = q.Text,
                QuizId = q.QuizId
            })
            .FirstOrDefaultAsync();
    }
    public async Task<QuestionDTO?> UpdateQuestionAsync(UpdateQuestionDTO updateQuestionDto, int userId)
    {
        var question = await _context.Questions.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == updateQuestionDto.Id && q.Quiz!.UserId == userId);
        if (question == null || question.Quiz!.UserId != userId)
        {
            return null;
        }
        question.Text = updateQuestionDto.Text;
        await _context.SaveChangesAsync();
        return new QuestionDTO
        {
            Id = question.Id,
            Text = question.Text,
            QuizId = question.QuizId
        };
    }
}
