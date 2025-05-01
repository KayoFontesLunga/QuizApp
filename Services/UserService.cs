using QuizApp.Data;
using QuizApp.DTOs;

namespace QuizApp.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterUser(UserRegistrationDTO userRegistrationDto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Email == userRegistrationDto.Email);
        if (userExists)
        {
            return false;
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password);
        var user = new User
        {
            Name = userRegistrationDto.Name,
            Email = userRegistrationDto.Email,
            Password = hashedPassword
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
