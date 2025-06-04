using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.DTOs.Answer;
using QuizApp.Models.Answers;

namespace QuizApp.Services;

public class AnswerService : IAnswerService
{
    private readonly AppDbContext _context;
    public AnswerService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<AnswerDTO?> CreateAnswerAsync(AnswerCreateDTO answerCreateDTO, int userId)
    {
        var question = await _context.Questions.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == answerCreateDTO.QuestionId && q.Quiz!.UserId == userId);
        if (question == null || question.Quiz!.UserId != userId)
        {
            return null;
        }
        var answer = new AnswersModel()
        {
            Text = answerCreateDTO.Text,
            IsCorrect = answerCreateDTO.IsCorrect,
            QuestionId = answerCreateDTO.QuestionId
        };
        _context.Answers.Add(answer);
        await _context.SaveChangesAsync();
        return new AnswerDTO()
        {
            Id = answer.Id,
            Text = answer.Text,
            IsCorrect = answer.IsCorrect,
            QuestionId = answer.QuestionId
        };
    }

    public async Task<List<AnswerDTO>?> GetAllAnswersByQuestionIdAsync(int questionId, int userId)
    {
        var question = await _context.Questions.Include(q => q.Quiz).FirstOrDefaultAsync(q => q.Id == questionId && q.Quiz!.UserId == userId);
        if (question == null || question.Quiz!.UserId != userId)
        {
            return null;
        }
        return await _context.Answers
            .Where(a => a.QuestionId == questionId)
            .Select(a => new AnswerDTO()
        {
            Id = a.Id,
            Text = a.Text,
            IsCorrect = a.IsCorrect,
            QuestionId = a.QuestionId
        }).ToListAsync();
    }

    public async Task<AnswerDTO?> UpdateAnswerAsync(AnswerUpdateDTO answerUpdateDTO, int userId)
    {
        var answer = await _context.Answers.Include(a => a.Questions).ThenInclude(q => q!.Quiz).FirstOrDefaultAsync(a => a.Id == answerUpdateDTO.Id && a.Questions!.Quiz!.UserId == userId);
        if (answer == null || answer.Questions!.Quiz!.UserId != userId)
        {
            return null;
        }
        answer.Text = answerUpdateDTO.Text;
        answer.IsCorrect = answerUpdateDTO.IsCorrect;
        await _context.SaveChangesAsync();
        return new AnswerDTO()
        {
            Id = answer.Id,
            Text = answer.Text,
            IsCorrect = answer.IsCorrect,
            QuestionId = answer.QuestionId
        };
    }
}
