using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public interface IUserService
    {
        Task<User> AddUser(User user, string password);
        Task<bool> UserExists(string id);
        Task<User> GetUser(string email);
        Task<User> AuthUser(string email, string password);
    }
}