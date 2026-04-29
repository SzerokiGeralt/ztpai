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
            _context.Products.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                var product = await GetProductByIdAsync(productId);
                _context.Products.Remove(product!);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FindAsync(productId);
        }

        public async Task UpdateProductAsync(Product request)
        {
            await _context.Products.Where(p => p.Id == request.Id).ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Price, request.Price)
                .SetProperty(p => p.Name, request.Name)
                .SetProperty(p => p.Description, request.Description));
        }
    }
}
