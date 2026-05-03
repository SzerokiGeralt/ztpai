using Microsoft.EntityFrameworkCore;
using ztpai.Migrations;
using ztpai.Models;

namespace ztpai.Repository
{
    public class UsersRepository(MyDbContext context) : IUsersRepository
    {
        public async Task AddUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await context.Users.FindAsync(userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.FirstAsync(x => x.Username == username);
        }

        public async Task RefreshTokenAsync(User user, string newRefreshToken, int days)
        {
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
            context.SaveChanges();
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return context.Users.Where(u =>  u.Username == username).Any();
        }
    }
}
