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

        public async Task<ProductResponseDTO?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            return new ProductResponseDTO
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }

        public async Task<bool> UpdateProductAsync(int id, ProductRequestDTO productDto)
        {
            var product = await _repository.GetProductByIdAsync(id);
            if(product == null)
            {
                return false; // not found
            }

            var updatedProduct = new Product
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };

            await _repository.UpdateProductAsync(updatedProduct);
            return true;
        }

        public async Task<Product> CreateProductAsync(ProductRequestDTO productDto)
        {
            var product = new Product 
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };

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
