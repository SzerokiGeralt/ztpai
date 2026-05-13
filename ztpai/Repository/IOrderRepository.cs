using ztpai.Models;

namespace ztpai.Repository
{
    public interface IOrderRepository
    {
        Task<int> CreateOrderAsync(Order order);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus);
        Task<Order?> GetOrderByIdAsync(int orderId);
    }
}
