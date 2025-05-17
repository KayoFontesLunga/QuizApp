using Microsoft.EntityFrameworkCore;
using QuizApp.Data;
using QuizApp.DTOs;
using QuizApp.Migrations;
using QuizApp.Models.User;

namespace QuizApp.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserListDTO>> GetAllUsers()
    {
        try
        {
            // Recupera todos os usuários do banco
            var users = await _context.Users.ToListAsync();

            // Transforma os usuários para o formato do DTO e retorna
            return users.Select(user => new UserListDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }).ToList();

        }
        catch (Exception ex)
        {

            return new List<UserListDTO>(); 
        }

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
            HashPassword = hashedPassword
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
