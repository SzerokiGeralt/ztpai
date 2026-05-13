using Moq;
using ztpai.DTO;
using ztpai.Models;
using ztpai.Repository;
using ztpai.Services;

namespace ztpai.UnitTests.ServicesTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IProductsRepository> _productsRepositoryMock;
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        public OrderServiceTests()
        {
            _productsRepositoryMock = new Mock<IProductsRepository>(MockBehavior.Strict);
            _orderRepositoryMock = new Mock<IOrderRepository>(MockBehavior.Strict);
        }

        [Fact]
        public async Task CreateOrderAsync_ProductNotFound_ReturnsError()
        {
            // Arrange
            var request = new CreateOrderRequestDTO
            {
                EmailAddress = "test@example.com",
                DeliveryAddress = "Address 1",
                Items = new List<OrderItemRequestDTO>
                {
                    new OrderItemRequestDTO { ProductId = 10, Quantity = 2 }
                }
            };

            _productsRepositoryMock
                .Setup(x => x.GetProductByIdAsync(10))
                .ReturnsAsync((Product?)null);

            var service = new OrderService(_productsRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await service.CreateOrderAsync(Guid.NewGuid(), request);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Product not found: 10", result.Error);
            Assert.Null(result.OrderId);

            _productsRepositoryMock.Verify(x => x.GetProductByIdAsync(10), Times.Once());
            _orderRepositoryMock.Verify(x => x.CreateOrderAsync(It.IsAny<Order>()), Times.Never());
        }

        [Fact]
        public async Task CreateOrderAsync_ValidRequest_ReturnsOrderIdAndStatus()
        {
            // Arrange
            var request = new CreateOrderRequestDTO
            {
                EmailAddress = "test@example.com",
                DeliveryAddress = "Address 1",
                Items = new List<OrderItemRequestDTO>
                {
                    new OrderItemRequestDTO { ProductId = 1, Quantity = 2 },
                    new OrderItemRequestDTO { ProductId = 2, Quantity = 1 }
                }
            };

            _productsRepositoryMock
                .Setup(x => x.GetProductByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1, Name = "A", Price = 10.0M });
            _productsRepositoryMock
                .Setup(x => x.GetProductByIdAsync(2))
                .ReturnsAsync(new Product { Id = 2, Name = "B", Price = 5.0M });

            _orderRepositoryMock
                .Setup(x => x.CreateOrderAsync(It.IsAny<Order>()))
                .ReturnsAsync(123);

            var service = new OrderService(_productsRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await service.CreateOrderAsync(Guid.NewGuid(), request);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(123, result.OrderId);
            Assert.Equal(OrderStatus.Ordered, result.Status);

            _productsRepositoryMock.Verify(x => x.GetProductByIdAsync(1), Times.Once());
            _productsRepositoryMock.Verify(x => x.GetProductByIdAsync(2), Times.Once());
            _orderRepositoryMock.Verify(x => x.CreateOrderAsync(It.Is<Order>(o =>
                o.OrderItems.Count == 2 &&
                o.TotalPrice == 25.0M &&
                o.EmailAddress == "test@example.com" &&
                o.DeliveryAddress == "Address 1")), Times.Once());
        }

        [Fact]
        public async Task UpdateOrderStatusAsync_ReturnsRepositoryResult()
        {
            // Arrange
            _orderRepositoryMock
                .Setup(x => x.UpdateOrderStatusAsync(10, OrderStatus.Confirmed))
                .ReturnsAsync(true);

            var service = new OrderService(_productsRepositoryMock.Object, _orderRepositoryMock.Object);

            // Act
            var result = await service.UpdateOrderStatusAsync(10, OrderStatus.Confirmed);

            // Assert
            Assert.True(result);
            _orderRepositoryMock.Verify(x => x.UpdateOrderStatusAsync(10, OrderStatus.Confirmed), Times.Once());
        }
    }
}
