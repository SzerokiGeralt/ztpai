using System.Net.Http.Json;
using ztpai.WebApp.Models;

namespace ztpai.WebApp.Services
{
    public class AuthApiService(HttpClient httpClient)
    {
        public async Task RegisterAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/register", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<TokenResponse?> LoginAsync(LoginRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/auth/login", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TokenResponse>();
        }

        public async Task UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            var response = await httpClient.PutAsJsonAsync("api/auth/password", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateRoleAsync(UpdateRoleRequest request)
        {
            var response = await httpClient.PutAsJsonAsync("api/auth/role", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task<IReadOnlyList<UserListItem>> GetUsersAsync()
        {
            var response = await httpClient.GetAsync("api/auth/users");
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                return Array.Empty<UserListItem>();
            }

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IReadOnlyList<UserListItem>>()
                ?? Array.Empty<UserListItem>();
        }
    }
}
