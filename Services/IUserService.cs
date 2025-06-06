using QuizApp.DTOs;
using QuizApp.DTOs.User;

namespace QuizApp.Services;

public interface IUserService
{
    Task<bool> RegisterUser(UserRegistrationDTO userRegistrationDto);
    Task<List<UserListDTO>> GetAllUsers();
    Task<string?> LoginAsync(UserLoginDTO userLoginDto);
}
