using Microsoft.EntityFrameworkCore;
using ztpai.Models;

namespace ztpai.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MyDbContext _context;

        public OrderRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.FindAsync(orderId);
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus)
        {
            var affected = await _context.Orders
                .Where(o => o.Id == orderId)
                .ExecuteUpdateAsync(u => u.SetProperty(o => o.Status, orderStatus));

            return affected > 0;
        }
    }
}
