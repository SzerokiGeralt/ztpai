using ztpai.Models;
using ztpai.Repository;
using ztpai.DTO;

namespace ztpai.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository _repository;

        public ProductsService(IProductsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetProductsAsync()
        {
            var products = await _repository.GetAllProductsAsync();
            var productsDto = products.Select(x => new ProductResponseDTO {
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            }).ToList();

            return productsDto;
        }
    }
}
