using System.Net.Http.Json;
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
    }
}
