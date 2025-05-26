using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs;
using QuizApp.Services;
using System.Security.Claims;

namespace QuizApp.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class QuizController : ControllerBase
{
    private readonly IQuizService _quizService;
    public QuizController(IQuizService quizService)
    {
        _quizService = quizService;
    }
    [HttpPost("CreateQuiz")]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizDTO createQuizDTO)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == (ClaimTypes.NameIdentifier))?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var result = await _quizService.CreateQuizAsync(createQuizDTO, userId);
            return Ok(result);

        }catch(Exception ex)
        {
            return BadRequest($"Error creating quiz: {ex.Message}");
        }
    }
    [HttpPut("UpdateQuiz")]
    public async Task<IActionResult> UpdateQuiz([FromBody] UpdateQuizDTO updateQuizDTO)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var result = await _quizService.UpdateQuizAsync(updateQuizDTO, userId);

            if (result == null)
            {
                return NotFound("Quiz not found or you are not the owner.");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating quiz: {ex.Message}");
        }
    }
    [HttpDelete("DeleteQuiz/{id}")]
    public async Task<IActionResult> DeleteQuiz(int id)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var result = await _quizService.DeleteQuizAsync(id, userId);
            if (!result)
                return NotFound("Quiz not encontrado ou você não tem permissão.");

            return NoContent(); 
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao deletar quiz: {ex.Message}");
        }
    }
}
