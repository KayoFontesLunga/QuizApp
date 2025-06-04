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
}
