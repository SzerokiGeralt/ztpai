using System.Security.Claims;
using ztpai.DTO;
using ztpai.Models;

namespace ztpai.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDTO request);
        Task<TokenResponseDTO?> LoginAsync(UserDTO request);
        Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request);
        Task UpdatePasswordAsync(UpdateUserPasswordDTO request, ClaimsPrincipal user);
        Task UpdateRoleAsync(UpdateUserRoleDTO request);
    }
}
