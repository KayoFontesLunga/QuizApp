using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs.Question;
using QuizApp.Services;
using System.Security.Claims;

namespace QuizApp.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuestionController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionController(IQuestionService questionService)
    {
        _questionService = questionService;
    }
    [HttpPost("CreateQuestion")]
    public async Task<IActionResult> CreateQuestionAsync([FromBody] QuestionCreateDTO createQuestionDto)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var createdQuestion = await _questionService.CreateQuestionAsync(createQuestionDto, userId);

            return Ok(createdQuestion);
        }catch (Exception ex)
        {
            return BadRequest($"Error creating question: {ex.Message}");
        }
    }
    [HttpGet("ByQuiz/{quizId}")] 
    public async Task<IActionResult> GetQuestionsByQuizAsync(int quizId)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var questions = await _questionService.GetAllQuestionsByQuizIdAsync(quizId, userId);
            if (questions == null)
            {
                return NotFound("Questions not found.");
            }
            return Ok(questions);
        }catch (Exception ex)
        {
            return BadRequest($"Error getting questions: {ex.Message}");
        }
    }
    [HttpGet("{id}")] 
    public async Task<IActionResult> GetQuestionByIdAsync(int id)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var question = await _questionService.GetQuestionByIdAsync(id, userId);
            if (question == null)
            {
                return NotFound("Question not found.");
            }
            return Ok(question);
        }catch (Exception ex)
        {
            return BadRequest($"Error getting question: {ex.Message}");
        }
    }
    [HttpPut("UpdateQuestion")]
    public async Task<IActionResult> UpdateQuestionAsync([FromBody] UpdateQuestionDTO updateQuestionDto)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var updatedQuestion = await _questionService.UpdateQuestionAsync(updateQuestionDto, userId);
            if (updatedQuestion == null)
            {
                return NotFound("Question not found.");
            }
            return Ok(updatedQuestion);
        }catch (Exception ex)
        {
            return BadRequest($"Error updating question: {ex.Message}");
        }
    }
    [HttpDelete("DeleteQuestion")]
    public async Task<IActionResult> DeleteQuestionAsync(int id)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var deletedQuestion = await _questionService.DeleteQuestionAsync(id, userId);
            if (!deletedQuestion)
            {
                return NotFound("Question not found.");
            }
            return Ok(deletedQuestion);
        }catch (Exception ex)
        {
            return BadRequest($"Error deleting question: {ex.Message}");
        }
    }
}
