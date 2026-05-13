using ztpai.DTO;
using ztpai.Models;
using ztpai.Repository;

namespace ztpai.Services
{
    public class OrderService(IProductsRepository productsRepository, IOrderRepository orderRepository) : IOrderService
    {
        public async Task<CreateOrderResultDTO> CreateOrderAsync(Guid userId, CreateOrderRequestDTO request)
        {
            var orderItems = new List<OrderItem>();
            foreach (var item in request.Items)
            {
                var product = await productsRepository.GetProductByIdAsync(item.ProductId);
                if (product is null)
                {
                    return new CreateOrderResultDTO
                    {
                        Success = false,
                        Error = $"Product not found: {item.ProductId}"
                    };
                }

                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Quantity = item.Quantity,
                    TotalPrice = product.Price * item.Quantity
                });
            }

            var order = new Order
            {
                UserId = userId,
                EmailAddress = request.EmailAddress ?? string.Empty,
                DeliveryAddress = request.DeliveryAddress ?? string.Empty,
                Created = DateTime.UtcNow,
                OrderItems = orderItems,
                TotalPrice = orderItems.Sum(i => i.TotalPrice),
                Status = OrderStatus.Ordered
            };

            var orderId = await orderRepository.CreateOrderAsync(order);

            return new CreateOrderResultDTO
            {
                Success = true,
                OrderId = orderId,
                Status = order.Status
            };
        }

        public Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            return orderRepository.UpdateOrderStatusAsync(orderId, status);
        }
    }
}
