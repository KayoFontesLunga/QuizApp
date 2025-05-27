using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QuizApp.Data;
using QuizApp.DTOs;
using QuizApp.Migrations;
using QuizApp.Models.User;
using QuizApp.Settings;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace QuizApp.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly JwtSettings _jwtSettings;
    public UserService(AppDbContext context, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _jwtSettings = jwtSettings.Value;
    }
    public async Task<List<UserListDTO>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();

        return users.Select(user => new UserListDTO()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        }).ToList();
    }
    public async Task<bool> RegisterUser(UserRegistrationDTO userRegistrationDto)
    {
        var userExists = await _context.Users.AnyAsync(u => u.Email == userRegistrationDto.Email);
        if (userExists)
        {
            return false;
        }
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegistrationDto.Password);
        var user = new UserModel()
        {
            Name = userRegistrationDto.Name,
            Email = userRegistrationDto.Email,
            HashPassword = hashedPassword,
            Role = "User",
            DateCreated = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<string?> LoginAsync(UserLoginDTO userLoginDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.HashPassword))
        {
            return null;
        }

        var claims = new[]
        {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name),
        new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
        new Claim(ClaimTypes.Role, user.Role ?? "User")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
