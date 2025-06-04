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
        };
    }
}
