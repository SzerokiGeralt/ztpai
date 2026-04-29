using Microsoft.EntityFrameworkCore;
using ztpai.Models;

namespace ztpai.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly MyDbContext _context;

        public ProductsRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product request)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateProductAsync(int productId, Product request)
        {
            throw new NotImplementedException();
        }
    }
}
