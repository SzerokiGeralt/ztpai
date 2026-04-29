using ztpai.DTO;
using ztpai.Models;

namespace ztpai.Mappers
{
    public static class ProductMapper
    {
        public static ProductResponseDTO ToProductResponseDTO(this Product product)
        {
            return new ProductResponseDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price
            };
        }

        public static Product ToProduct(this ProductRequestDTO productDto, int id = 0)
        {
            return new Product
            {
                Id = id,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price
            };
        }
    }
}