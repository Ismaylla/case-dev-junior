namespace TaskApi.UnitTests.Service
{
    public class AuthServiceTests
    {
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private const string JWT_SECRET = "test-secret-key-12345";

        public AuthServiceTests()
        {
            _userService = new UserService();
            _authService = new AuthService(_userService, JWT_SECRET);
        }

        [Fact]
        public void ValidateUser_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            var user = _userService.CreateUser(email, "Test User", password);

            // Act
            var validatedUser = _authService.ValidateUser(email, password);

            // Assert
            Assert.NotNull(validatedUser);
            Assert.Equal(user.Id, validatedUser.Id);
            Assert.Equal(user.Email, validatedUser.Email);
        }

        [Fact]
        public void ValidateUser_WithInvalidEmail_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            _userService.CreateUser(email, "Test User", password);

            // Act
            var validatedUser = _authService.ValidateUser("wrong@example.com", password);

            // Assert
            Assert.Null(validatedUser);
        }

        [Fact]
        public void ValidateUser_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123!";
            _userService.CreateUser(email, "Test User", password);

            // Act
            var validatedUser = _authService.ValidateUser(email, "WrongPassword123!");

            // Assert
            Assert.Null(validatedUser);
        }
    }
}
