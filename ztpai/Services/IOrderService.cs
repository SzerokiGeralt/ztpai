using ztpai.DTO;
using ztpai.Models;

namespace ztpai.Services
{
    public interface IOrderService
    {
        Task<CreateOrderResultDTO> CreateOrderAsync(Guid userId, CreateOrderRequestDTO request);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
    }
}
