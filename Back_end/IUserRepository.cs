using WebShop.Repos.Models;

namespace Webshop.Data
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User newUser);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int id);
        Task SavePasswordResetTokenAsync(int userId, string token, DateTime expiration);
        Task<User?> GetUserByPasswordResetTokenAsync(string token);
        Task UpdateAsync(User user);
    }
}