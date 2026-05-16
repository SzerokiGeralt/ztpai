using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using ztpai.WebApp.Models;

namespace ztpai.WebApp.Services
{
    public class ProductsApiService(HttpClient httpClient)
    {
        public async Task<IReadOnlyList<ProductResponse>> GetProductsAsync()
        {
            return await httpClient.GetFromJsonAsync<IReadOnlyList<ProductResponse>>("api/products") ?? Array.Empty<ProductResponse>();
        }

        public async Task<ProductResponse?> GetProductAsync(int id)
        {
            return await httpClient.GetFromJsonAsync<ProductResponse>($"api/products/{id}");
        }

        public async Task CreateProductAsync(ProductRequest request)
        {
            var response = await httpClient.PostAsJsonAsync("api/products", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(int id, ProductRequest request)
        {
            var response = await httpClient.PutAsJsonAsync($"api/products/{id}", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<string> UploadImageAsync(IBrowserFile file)
        {
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024));
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
            content.Add(streamContent, "file", file.Name);

            var response = await httpClient.PostAsync("api/images/upload", content);
            response.EnsureSuccessStatusCode();

            var payload = await response.Content.ReadFromJsonAsync<ImageUploadResponse>();
            if (payload is null || string.IsNullOrWhiteSpace(payload.FileName))
            {
                throw new InvalidOperationException("Brak nazwy pliku z serwera.");
            }

            return payload.FileName;
        }

        public async Task DeleteImageAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return;
            }

            var response = await httpClient.DeleteAsync($"api/images/{Uri.EscapeDataString(fileName)}");
            response.EnsureSuccessStatusCode();
        }

        private sealed class ImageUploadResponse
        {
            public string? FileName { get; set; }
        }
    }
}
