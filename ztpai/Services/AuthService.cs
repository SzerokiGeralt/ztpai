using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ztpai.DTO;
using ztpai.Models;
using ztpai.Repository;

namespace ztpai.Services
{
    public class AuthService(IUsersRepository usersRepository, IConfiguration configuration) : IAuthService
    {
        public async Task<TokenResponseDTO?> LoginAsync(UserDTO request)
        {
            var user = await usersRepository.GetUserByUsernameAsync(request.Username);
            if (user is null)
            {
                throw new ArgumentException("User not found");
            }
            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Wrong password");
            }

            return await CreateResponseToken(user);
        }

        private async Task<TokenResponseDTO> CreateResponseToken(User user)
        {
            return new TokenResponseDTO
            {
                AccessToken = CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshToken(user),
            };
        }

        public async Task<User?> RegisterAsync(UserDTO request)
        {
            if (await usersRepository.UsernameExistsAsync(request.Username))
            {
                throw new ArgumentException("User already exists");
            }

            var user = new User();

            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);
            user.Username = request.Username;
            user.PasswordHash = hashedPassword;

            await usersRepository.AddUserAsync(user);

            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role),
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!)
                );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var random = RandomNumberGenerator.Create();
            random.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken();
            try
            {
                await usersRepository.RefreshTokenAsync(user, refreshToken, 7);
            }
            catch (Exception)
            {
                throw;
            }
            return refreshToken;
        }

        public async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var user = await usersRepository.GetUserByIdAsync(userId);
            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiration <= DateTime.UtcNow) { return null; }
            return user;
        }

        public async Task<TokenResponseDTO?> RefreshTokensAsync(RefreshTokenRequestDTO request)
        {
            var user = await ValidateRefreshTokenAsync(request.UserID, request.RefreshToken);
            if (user is null) return null;
            return await CreateResponseToken(user);
        }

        public async Task UpdatePasswordAsync(UpdateUserPasswordDTO request, ClaimsPrincipal user)
        {
            var usernameClaim = user.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrWhiteSpace(usernameClaim) || !string.Equals(usernameClaim, request.Username, StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("Cannot change password for another user");
            }

            var existingUser = await usersRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser is null)
            {
                throw new ArgumentException("User not found");
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(existingUser, existingUser.PasswordHash, request.CurrentPassword) == PasswordVerificationResult.Failed)
            {
                throw new ArgumentException("Wrong password");
            }

            existingUser.PasswordHash = new PasswordHasher<User>().HashPassword(existingUser, request.NewPassword);
            await usersRepository.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(UpdateUserRoleDTO request)
        {
            var existingUser = await usersRepository.GetUserByUsernameAsync(request.Username);
            if (existingUser is null)
            {
                throw new ArgumentException("User not found");
            }

            existingUser.Role = request.Role;
            await usersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserListItemDTO>?> ListAllUsersAsync()
        {
            var users = await usersRepository.GetUsersAsync();
            return users?.Select(user => new UserListItemDTO
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            });
        }
    }
}
