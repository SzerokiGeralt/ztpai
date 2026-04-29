using ztpai.Models;
using ztpai.Repository;

namespace ztpai.Services
{
    public class OrderService
    {
        private readonly IProductsRepository _repository;

        public OrderService(IProductsRepository repository)
        {
            _repository = repository;
        }
        public decimal calculateTotal(List<Product> products)
        {
            if (products is null) throw new ArgumentNullException("Calculate total list null reference");
            decimal sum = 0.0M;
            sum = products.Select(x => x.Price).ToList().Sum();
            return sum;
        }
    }
}
