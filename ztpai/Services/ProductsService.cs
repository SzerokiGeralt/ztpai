using ztpai.Models;
using ztpai.Repository;
using ztpai.DTO;
using ztpai.Mappers;

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
            var productsDto = products.Select(x => x.ToProductResponseDTO()).ToList();

            return productsDto;
        }

        public async Task<ProductResponseDTO?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            return product.ToProductResponseDTO();
        }

        public async Task<bool> UpdateProductAsync(int id, ProductRequestDTO productDto)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if(product == null)
            {
                return false; // not found
            }

            var updatedProduct = productDto.ToProduct(id);

            await _repository.UpdateProductAsync(updatedProduct);
            return true;
        }

        public async Task<Product> CreateProductAsync(ProductRequestDTO productDto)
        {
            var product = productDto.ToProduct();

            await _repository.AddProductAsync(product);
            return product; // To return the generated Id
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            await _repository.DeleteProductAsync(id);
            return true;
        }
    }
}
