
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ztpai.Models;
using ztpai.Services;

namespace ztpai.UnitTests.ServicesTests
{
    public class AuthServiceTests
    {
        private readonly MyDbContext _inMemoryContext;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly string _mockToken = "mockToken" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(255));
        public AuthServiceTests()
        {
            _mockConfiguration = new(MockBehavior.Strict);
            _mockConfiguration.Setup(x => x.GetValue<string>("AppSettings:Token")).Returns(_mockToken);
            _mockConfiguration.Setup(x => x.GetValue<string>("AppSettings:Audience")).Returns("mockAudience");
            _mockConfiguration.Setup(x => x.GetValue<string>("AppSettings:Issuer")).Returns("mockIssuer");
        }

        //TODO: AuthServiceTest1
        [Fact]
        public void Login_UserNotFound_ThrowsException()
        {
            //Arrange

            //Act

            //Assert

        }

        //TODO: AuthServiceTest2
        [Fact]
        public void Login_WrongPassowrd_ThrowsException()
        {
            //Arrange
            //Act
            //Assert
        }

        //TODO: AuthServiceTest3
        [Fact]
        public void Login_Correct_ReturnsResponseToken()
        {
            //Arrange
            //Act
            //Assert
        }

        //TODO: AuthServiceTest4
        [Fact]
        public void RefreshTokenGeneration_CorrectUser_UpdatesExpirationDate()
        {
            //Arrange
            //Act
            //Assert
        }

        //TODO: AuthServiceTest5
        [Fact]
        public void RefreshTokenGeneration_InvalidUser_ThrowsException()
        {
            //Arrange
            //Act
            //Assert
        }

        //TODO: AuthServiceTest6
        [Fact]
        public void Registration_UserExists_ThrowsException()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}
