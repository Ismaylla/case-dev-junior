namespace TaskApi.UnitTests.TestDoubles
{
    public class TestUserService : ITestUserService
    {
        private readonly Dictionary<string, User> _users;

        public TestUserService()
        {
            _users = new Dictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        }

        public User CreateUser(string email, string name, string password)
        {
            if (_users.ContainsKey(email))
                throw new Exception("Email já está em uso.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                Name = name,
                PasswordHash = "hashed_" + password,
                Role = "User"
            };

            _users[email] = user;
            return user;
        }

        public User? GetUser(string email)
        {
            _users.TryGetValue(email, out var user);
            return user;
        }
    }
}
