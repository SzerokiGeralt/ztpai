using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using ztpai.Models;
using ztpai.Repository;
using ztpai.Services;

namespace ztpai.UnitTests.ServicesTests
{
    public class OrderServiceTests
    {
        private readonly Mock<IProductsRepository> _repositoryMock;
        public OrderServiceTests()
        {
            _repositoryMock = new Mock<IProductsRepository>(MockBehavior.Strict);
        }

        [Fact]
        public void CalculateTotal_ThreeProducts_ReturnsCorrectSum()
        {
            //Arrange
            var productList = new List<Product>
            {
                new Product {Price = 10.0M},
                new Product {Price = 20.0M},
                new Product {Price = 30.0M}
            };
            //Act
            var service = new OrderService(_repositoryMock.Object);
            var result = service.calculateTotal(productList);
            //Assert
            Assert.Equal(60.0M,result);
        }

        [Fact]
        public void CalculateTotal_EmptyList_ReturnsZero()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public void CalculateTotal_NullArgument_ThrowsException()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
