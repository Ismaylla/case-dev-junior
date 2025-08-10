public class UserService
{
    private readonly List<User> _users = new();

    public User? GetByEmail(string email) =>
        _users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

    public User CreateUser(string email, string name, string password)
    {
        if (GetByEmail(email) != null)
            throw new Exception("Email já está em uso.");

        var user = new User
        {
            Email = email,
            Name = name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Role = "User"
        };

        _users.Add(user);
        return user;
    }

    public IEnumerable<User> GetAll() => _users;
}
