using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizApp.Data;
using QuizApp.DTOs;

namespace QuizApp.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;
    public UserController(AppDbContext context)
    {
        _context = context;
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
            var emailExists = await _context.Users.AnyAsync(u => u.Email == userRegistrationDto.Email);

            if (emailExists)
            {
                return BadRequest("Email already exists");
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password);

            // TODO: Create new User object (when the model is ready)
            var newUser = new User
            {
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                Password = hashedPassword
                 DateCreated = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            return Ok("User registered successfully");

        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error registering user: {ex.Message}");
        }
    }
}
