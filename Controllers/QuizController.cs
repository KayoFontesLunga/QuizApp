using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs;
using QuizApp.DTOs.Paginated;
using QuizApp.DTOs.Quiz;
using QuizApp.DTOs.Submit;
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
                return NotFound("Quiz not found or you don't have permission.");

            return NoContent(); 
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting quiz: {ex.Message}");
        }
    }

    [HttpGet("Quizzes")]
    public async Task<IActionResult> GetMyQuizzes()
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var quizzes = await _quizService.GetAllQuizzesByUserAsync(userId);
            return Ok(quizzes);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao buscar quizzes: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetQuizById(int id)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }

            var quiz = await _quizService.GetQuizByIdAsync(id, userId);
            if (quiz == null)
                return NotFound("Quiz não encontrado ou você não tem permissão.");

            return Ok(quiz);
        }
        catch (Exception ex)
        {
            return BadRequest($"Erro ao buscar quiz: {ex.Message}");
        }
    }
    [HttpPost("submit")]
    public async Task<ActionResult<QuizResultDTO>> SubmitQuiz([FromBody] SubmitQuizDTO submitQuizDTO)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var result = await _quizService.SubmitQuizAsync(submitQuizDTO, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error submitting quiz: {ex.Message}");
        }
    }
    [HttpGet("{quizId}/ranking")]
    public async Task<ActionResult<List<QuizRankingDTO>>> GetRankingByQuizId(int quizId)
    {
        try
        {
            var ranking = await _quizService.GetRankingByQuizIdAsync(quizId);
            return Ok(ranking);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting ranking: {ex.Message}");
        }
    }
    [HttpGet("public")]
    public async Task<ActionResult<PaginatedResult<QuizDTO>>> GetPublicQuizzes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var quizzes = await _quizService.GetPublicQuizzesPaginatedAsync(page, pageSize);
            return Ok(quizzes);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting public quizzes: {ex.Message}");
        }
    }
}
