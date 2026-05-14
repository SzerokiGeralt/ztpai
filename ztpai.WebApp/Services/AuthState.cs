using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.JSInterop;
using ztpai.WebApp.Models;

namespace ztpai.WebApp.Services
{
    public class AuthState(IJSRuntime jsRuntime)
    {
        private const string TokenKey = "auth_token";
        private const string UsernameKey = "auth_username";
        private const string UserIdKey = "auth_userid";

        public string? AccessToken { get; private set; }
        public string? Username { get; private set; }
        public Guid? UserId { get; private set; }
        public string? Role { get; private set; }
        public bool IsAdmin => string.Equals(Role, "Admin", StringComparison.OrdinalIgnoreCase);

        public bool IsAuthenticated => !string.IsNullOrWhiteSpace(AccessToken);

        public event Action? OnChange;

        public async Task InitializeAsync()
        {
            AccessToken = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
            Username = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", UsernameKey);
            var userIdValue = await jsRuntime.InvokeAsync<string?>("localStorage.getItem", UserIdKey);
            if (Guid.TryParse(userIdValue, out var parsedId))
            {
                UserId = parsedId;
            }

            if (!string.IsNullOrWhiteSpace(AccessToken))
            {
                Role = ReadClaim(AccessToken, ClaimTypes.Role);
            }

            OnChange?.Invoke();
        }

        public async Task SetSessionAsync(TokenResponse token)
        {
            AccessToken = token.AccessToken;
            Username = ReadClaim(token.AccessToken, ClaimTypes.Name);
            Role = ReadClaim(token.AccessToken, ClaimTypes.Role);
            var userIdClaim = ReadClaim(token.AccessToken, ClaimTypes.NameIdentifier);
            UserId = Guid.TryParse(userIdClaim, out var parsedId) ? parsedId : null;

            await jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token.AccessToken);
            if (Username is not null)
            {
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", UsernameKey, Username);
            }

            if (UserId is not null)
            {
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", UserIdKey, UserId.ToString());
            }

            OnChange?.Invoke();
        }

        public async Task ClearAsync()
        {
            AccessToken = null;
            Username = null;
            UserId = null;
            Role = null;
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", UsernameKey);
            await jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserIdKey);
            OnChange?.Invoke();
        }

        private static string? ReadClaim(string jwt, string claimType)
        {
            var parts = jwt.Split('.');
            if (parts.Length < 2)
            {
                return null;
            }

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');
            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            using var document = JsonDocument.Parse(json);
            if (!document.RootElement.TryGetProperty(claimType, out var value))
            {
                var fallback = claimType.Split('/').LastOrDefault();
                if (fallback is null || !document.RootElement.TryGetProperty(fallback, out value))
                {
                    return null;
                }
            }

            return value.GetString();
        }
    }
}
