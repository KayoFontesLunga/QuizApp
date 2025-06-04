using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.DTOs.Answer;
using QuizApp.Services;
using System.Security.Claims;

namespace QuizApp.Controllers;
[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnswerController : ControllerBase
{
    private readonly IAnswerService _answerService;
    public AnswerController(IAnswerService answerService)
    {
        _answerService = answerService;
    }
    [HttpPost("CreateAnswer")]
    public async Task<IActionResult> CreateAnswer([FromBody] AnswerCreateDTO createAnswerDTO)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var createdAnswer = await _answerService.CreateAnswerAsync(createAnswerDTO, userId);
            return Ok(createdAnswer);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error creating answer: {ex.Message}");
        }
    }
    [HttpGet("question/{questionId}")]
    public async Task<IActionResult> GetAnswersByQuestionId(int questionId)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var answers = await _answerService.GetAllAnswersByQuestionIdAsync(questionId, userId);
            return Ok(answers); 
        }
        catch (Exception ex)
        {
            return BadRequest($"Error getting answers: {ex.Message}");
        }
    }
    [HttpPut("UpdateAnswer")]
    public async Task<IActionResult> UpdateAnswer([FromBody] AnswerUpdateDTO updateAnswerDTO)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var updatedAnswer = await _answerService.UpdateAnswerAsync(updateAnswerDTO, userId);
            return Ok(updatedAnswer);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error updating answer: {ex.Message}");
        }
    }
    [HttpDelete("DeleteAnswer/{id}")] 
    public async Task<IActionResult> DeleteAnswer(int id)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized("User ID not found in claims.");
            }
            var deletedAnswer = await _answerService.DeleteAnswerAsync(id, userId);
            if (!deletedAnswer)
            {
                return NotFound("Answer not found.");
            }
            return Ok(deletedAnswer);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error deleting answer: {ex.Message}");
        }
    }
}
