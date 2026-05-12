using Microsoft.AspNetCore.Http;

namespace ztpai.Services
{
    public interface IProductImageService
    {
        Task<string> UploadProductImageAsync(IFormFile file);
        Task DeleteProductImageAsync(string fileName);
    }
}
