using System.Collections.ObjectModel;
using ztpai.DTO;
using ztpai.Models;

namespace ztpai.Repository
{
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int productId);
        Task UpdateProductAsync(Product request);
        Task DeleteProductAsync(int productId);
        Task AddProductAsync(Product request);
    }
}
