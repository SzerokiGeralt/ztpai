using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ztpai.WebApp.Services;

namespace ztpai.WebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped<AuthState>();
            builder.Services.AddScoped<AuthenticatedHttpClientHandler>();

            builder.Services.AddScoped(sp =>
            {
                var handler = sp.GetRequiredService<AuthenticatedHttpClientHandler>();
                handler.InnerHandler = new HttpClientHandler();
                var client = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:7094/")
                };
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            });

            builder.Services.AddScoped<AuthApiService>();
            builder.Services.AddScoped<ProductsApiService>();

            await builder.Build().RunAsync();
        }
    }
}
