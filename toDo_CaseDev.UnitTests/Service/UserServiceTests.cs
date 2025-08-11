using Xunit;
using TaskApi.Models;
using System;

namespace TaskApi.UnitTests.Service
{
    public class UserServiceTests
    {
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userService = new UserService();
        }

        [Fact]
        public void CreateUser_WithValidData_CreatesUser()
        {
            // Arrange
            var email = "test@example.com";
            var name = "Test User";
            var password = "Password123!";

            // Act
            var user = _userService.CreateUser(email, name, password);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(email, user.Email);
            Assert.Equal(name, user.Name);
            Assert.NotNull(user.PasswordHash);
            Assert.Equal("User", user.Role);
        }

        [Fact]
        public void CreateUser_WithDuplicateEmail_ThrowsException()
        {
            // Arrange
            var email = "test@example.com";
            var name = "Test User";
            var password = "Password123!";
            _userService.CreateUser(email, name, password);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _userService.CreateUser(email, name, password));
            Assert.Equal("Email já está em uso.", ex.Message);
        }

        [Fact]
        public void GetByEmail_WithExistingEmail_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var name = "Test User";
            var password = "Password123!";
            var createdUser = _userService.CreateUser(email, name, password);

            // Act
            var user = _userService.GetByEmail(email);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(createdUser.Id, user.Id);
            Assert.Equal(email, user.Email);
            Assert.Equal(name, user.Name);
        }

        [Fact]
        public void GetByEmail_WithNonExistentEmail_ReturnsNull()
        {
            // Act
            var user = _userService.GetByEmail("nonexistent@example.com");

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public void GetAll_ReturnsAllUsers()
        {
            // Arrange
            var user1 = _userService.CreateUser("test1@example.com", "Test User 1", "Password123!");
            var user2 = _userService.CreateUser("test2@example.com", "Test User 2", "Password123!");

            // Act
            var users = _userService.GetAll();

            // Assert
            Assert.NotNull(users);
            Assert.Contains(users, u => u.Id == user1.Id);
            Assert.Contains(users, u => u.Id == user2.Id);
        }
    }
}
