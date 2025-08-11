namespace TaskApi.UnitTests.TestDoubles
{
    public class TestAuthService : ITestAuthService
    {
        private readonly TestUserService _userService;
        private const string _jwtSecret = "test-jwt-secret-key-1234567890-minimum-32-chars";
        
        public TestAuthService()
        {
            _userService = new TestUserService();
        }

        public User? ValidateUser(string email, string password)
        {
            var user = _userService.GetUser(email);
            if (user == null) return null;

            var hashedPassword = "hashed_" + password;
            return hashedPassword == user.PasswordHash ? user : null;
        }

        public string GenerateJwtToken(User user)
        {
            return $"test_token_for_{user.Email}";
        }
    }
}
