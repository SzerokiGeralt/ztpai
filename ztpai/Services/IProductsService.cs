using ztpai.DTO;
using ztpai.Models;

namespace ztpai.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductResponseDTO>> GetProductsAsync();
        Task<ProductResponseDTO?> GetProductByIdAsync(int id);
        Task<bool> UpdateProductAsync(int id, ProductRequestDTO productDto);
        Task<Product> CreateProductAsync(ProductRequestDTO productDto);
        Task<bool> DeleteProductAsync(int id);
    }
}