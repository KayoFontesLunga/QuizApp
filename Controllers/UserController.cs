using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Data;
using QuizApp.DTOs;
using QuizApp.Services;

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
}
