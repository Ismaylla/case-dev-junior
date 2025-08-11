using TaskApi.Models;

namespace TaskApi.UnitTests.TestDoubles
{
    public interface ITestAuthService
    {
        User? ValidateUser(string email, string password);
        string GenerateJwtToken(User user);
    }
}
