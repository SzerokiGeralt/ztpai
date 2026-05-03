using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ztpai.Models;
using ztpai.Repository;
using ztpai.Services;

namespace ztpai.UnitTests.ServicesTests
{
    public class ProductsServiceTests
    {
        private readonly Mock<IProductsRepository> _mockRepository;

        public ProductsServiceTests()
        {
            _mockRepository = new Mock<IProductsRepository>(MockBehavior.Strict);
        }

        [Fact]
        public async Task GetProductById_WithValidId_ReturnsNotNullName()
        {
            //Arrange
            _mockRepository.Setup(x => x.GetProductByIdAsync(1))
                .ReturnsAsync(new Product { 
                    Id = 1, 
                    Name = "Mock product", 
                    Description = "Mock product desc", 
                    Price = 10.0M });
            var productsService = new ProductsService(_mockRepository.Object);

            //Act
            var result = await productsService.GetProductByIdAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Mock product", result.Name);

            // Verify
            _mockRepository.Verify(x => x.GetProductByIdAsync(1), Times.Once());
        }

        [Fact]
        public async Task GetProductById_WithWrongId_ReturnsNull()
        {
            //Arrange
            _mockRepository.Setup(x => x.GetProductByIdAsync(99))
                .ReturnsAsync((Product)null!);
            //Act
            var productsService = new ProductsService(_mockRepository.Object);
            var result = await productsService.GetProductByIdAsync(99);

            //Assert
            Assert.Null(result);

            //Verify
            _mockRepository.Verify(x => x.GetProductByIdAsync(99), Times.Once());
        }

        [Fact]
        public async Task CreateProductAsync_ValidProduct_ReturnsProductWithId()
        {
            // Arrange
            var requestDto = new DTO.ProductRequestDTO
            {
                Name = "New Product",
                Description = "Description",
                Price = 15.0M
            };

            _mockRepository.Setup(x => x.AddProductAsync(It.IsAny<Product>()))
                .Callback<Product>(p => p.Id = 123)
                .Returns(Task.CompletedTask); 

            var productsService = new ProductsService(_mockRepository.Object);

            // Act
            var result = await productsService.CreateProductAsync(requestDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.Id);

            // Verify
            _mockRepository.Verify(x => x.AddProductAsync(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public async Task CreateProductAsync_NullName_ThrowsArgumentException()
        {
            // Arrange
            var requestDto = new DTO.ProductRequestDTO
            {
                Name = null,
                Description = "Description",
                Price = 15.0M
            };

            var productsService = new ProductsService(_mockRepository.Object);

            // Act
            var exceptionCode = () => productsService.CreateProductAsync(requestDto);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(exceptionCode);
            Assert.Equal("Product name cannot be null or empty.", ex.Message);
        }

        [Fact]
        public async Task CreateProductAsync_EmptyName_ThrowsArgumentException()
        {
            // Arrange
            var requestDto = new DTO.ProductRequestDTO
            {
                Name = string.Empty,
                Description = "Description",
                Price = 15.0M
            };

            var productsService = new ProductsService(_mockRepository.Object);

            // Act
            var exceptionCode = () => productsService.CreateProductAsync(requestDto);

            // Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(exceptionCode);
            Assert.Equal("Product name cannot be null or empty.", ex.Message);
        }
    }
}
