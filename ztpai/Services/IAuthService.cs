using ztpai.DTO;
using ztpai.Models;

namespace ztpai.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO request);
        Task<string?> LoginAsync(UserDTO request);
    }
}
