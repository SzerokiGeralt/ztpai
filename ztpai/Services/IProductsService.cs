using ztpai.DTO;

namespace ztpai.Services
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductResponseDTO>> GetProductsAsync();
    }
}