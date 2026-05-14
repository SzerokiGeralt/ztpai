using System.Net.Http.Headers;

namespace ztpai.WebApp.Services
{
    public class AuthenticatedHttpClientHandler(AuthState authState) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(authState.AccessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authState.AccessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
