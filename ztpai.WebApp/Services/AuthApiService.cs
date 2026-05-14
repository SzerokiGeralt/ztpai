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
    }
}
