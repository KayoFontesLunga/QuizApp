using QuizApp.DTOs;

namespace QuizApp.Services;

public interface IUserService
{
    Task<bool> RegisterUser(UserRegistrationDTO userRegistrationDto);
    Task<List<UserListDTO>> GetAllUsers();
}
