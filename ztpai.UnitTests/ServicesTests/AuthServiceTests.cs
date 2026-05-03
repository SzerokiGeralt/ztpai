

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using ztpai.DTO;
using ztpai.Models;
using ztpai.Repository;
using ztpai.Services;

namespace ztpai.UnitTests.ServicesTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IUsersRepository> _mockUserRepository;
        private readonly IConfiguration _mockConfiguration;
        private readonly string _mockToken = "mockToken" + Convert.ToBase64String(RandomNumberGenerator.GetBytes(255));
        public AuthServiceTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"AppSettings:Token", _mockToken},
                {"AppSettings:Issuer", "my-test-issuer"},
                {"AppSettings:Audience", "my-test-audience"}
            };

            _mockConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration!)
                .Build();

            _mockUserRepository = new(MockBehavior.Strict);

        }

        [Fact]
        public async Task Login_UserNotFound_ThrowsException()
        {
            //Arrange
            _mockUserRepository.Setup(r => r.GetUserByUsernameAsync("admin")).ReturnsAsync((User)null!);
            var service = new AuthService(_mockUserRepository.Object, _mockConfiguration);
            UserDTO userLogin = new UserDTO { Password = "correct_password", Username = "admin" };

            //Act
            var exceptionCode = () => service.LoginAsync(userLogin);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(exceptionCode);
            Assert.Equal("User not found", ex.Message);
        }

        [Fact]
        public async Task Login_WrongPassword_ThrowsException()
        {
            //Arrange
            var userRepo = new User();
            userRepo.Username = "admin";
            userRepo.PasswordHash = new PasswordHasher<User>().HashPassword(userRepo, "correct_password");
            
            _mockUserRepository.Setup(r => r.GetUserByUsernameAsync("admin")).ReturnsAsync(userRepo);
            var service = new AuthService(_mockUserRepository.Object, _mockConfiguration);
            UserDTO userLogin = new UserDTO { Password = "wrong_password", Username = "admin" };

            //Act
            var exceptionCode = () => service.LoginAsync(userLogin);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(exceptionCode);
            Assert.Equal("Wrong password", ex.Message);
        }

        [Fact]
        public async Task Login_Correct_ReturnsResponseToken()
        {
            //Arrange
            var userRepo = new User();
            userRepo.Username = "admin";
            userRepo.PasswordHash = new PasswordHasher<User>().HashPassword(userRepo, "correct_password");

            _mockUserRepository.Setup(r => r.GetUserByUsernameAsync("admin")).ReturnsAsync(userRepo);
            var service = new AuthService(_mockUserRepository.Object, _mockConfiguration);
            UserDTO userLogin = new UserDTO { Password = "correct_password", Username = "admin" };

            _mockUserRepository.Setup(r => r.RefreshTokenAsync(userRepo, It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            //Act
            var result = await service.LoginAsync(userLogin);

            //Assert
            Assert.IsType<TokenResponseDTO>(result);
        }

        [Fact]
        public async Task RefreshTokensAsync_TokenValid_ReturnsResponse()
        {
            //Arrange
            var userGuid = Guid.NewGuid();
            var userRepo = new User();
            userRepo.Username = "admin";
            userRepo.PasswordHash = new PasswordHasher<User>().HashPassword(userRepo, "correct_password");
            userRepo.RefreshToken = "123";
            userRepo.RefreshTokenExpiration = DateTime.UtcNow.AddDays(1);

            var service = new AuthService(_mockUserRepository.Object, _mockConfiguration);
            RefreshTokenRequestDTO refresh = new() { RefreshToken = "123", UserID = userGuid };

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(userGuid)).ReturnsAsync(userRepo);
            _mockUserRepository.Setup(r => r.RefreshTokenAsync(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<int>())).Returns(Task.CompletedTask);

            //Act
            var result = await service.RefreshTokensAsync(refresh);

            //Assert
            Assert.IsType<TokenResponseDTO>(result);
        }

        [Fact]
        public async Task RefreshTokensAsync_TokenExpired_ReturnsNull()
        {
            //Arrange
            var userGuid = Guid.NewGuid();
            var userRepo = new User();
            userRepo.Username = "admin";
            userRepo.PasswordHash = new PasswordHasher<User>().HashPassword(userRepo, "correct_password");
            userRepo.RefreshToken = "123";
            userRepo.RefreshTokenExpiration = DateTime.UtcNow.AddDays(-1);

            var service = new AuthService(_mockUserRepository.Object, _mockConfiguration);
            RefreshTokenRequestDTO refresh = new() { RefreshToken = "123", UserID = userGuid };

            _mockUserRepository.Setup(r => r.GetUserByIdAsync(userGuid)).ReturnsAsync(userRepo);

            //Act
            var result = await service.RefreshTokensAsync(refresh);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Registration_UserExists_ThrowsException()
        {
            //Arrange
            _mockUserRepository.Setup(r => r.UsernameExistsAsync("admin")).ReturnsAsync(true);
            var service = new AuthService(_mockUserRepository.Object, _mockConfiguration);
            UserDTO userRegister = new UserDTO { Password = "correct_password", Username = "admin" };

            //Act
            var exceptionCode = () => service.RegisterAsync(userRegister);

            //Assert
            var ex = await Assert.ThrowsAsync<ArgumentException>(exceptionCode);
            Assert.Equal("User already exists", ex.Message);
        }
    }
}
