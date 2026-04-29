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

        public Task AddProductAsync(Product request)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductAsync(int productId, Product request)
        {
            throw new NotImplementedException();
        }
    }
}
