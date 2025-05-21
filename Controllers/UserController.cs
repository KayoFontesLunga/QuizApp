using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Data;
using QuizApp.DTOs;
using QuizApp.Services;
using System.Security.Claims;

namespace QuizApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO userRegistrationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _userService.RegisterUser(userRegistrationDto);
            if (result)
            {
                return Ok("User registered successfully.");
            }
            else
            {
                return BadRequest("User registration failed.");
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error registering user: {ex.Message}");
        }
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();

        if (users == null || !users.Any())
            return NotFound("Nenhum usuário encontrado.");

        return Ok(users); 
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var token = await _userService.LoginAsync(userLoginDto);
            if (token == null)
            {
                return Unauthorized("credentials invalid.");
            }
            else
            {
                return Ok(new { token });
            }
        }catch (Exception ex)
        {
            return StatusCode(500, $"Error logging in: {ex.Message}");
        }
    }
    [Authorize]
    [HttpGet("WhoAmI")]
    public IActionResult WhoAmI()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userName = User.Identity?.Name;
        return Ok(new { userId, userName });
    }
}
