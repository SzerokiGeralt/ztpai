using ztpai.Models;

namespace ztpai.Repository
{
    public interface IUsersRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UsernameExistsAsync(string username);
        Task AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(Guid userId);
        Task RefreshTokenAsync(User user, string newRefreshToken, int days);
        Task SaveChangesAsync();
    }
}
