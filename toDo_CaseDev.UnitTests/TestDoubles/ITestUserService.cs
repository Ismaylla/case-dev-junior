namespace TaskApi.UnitTests.TestDoubles
{
    public interface ITestUserService
    {
        User? GetUser(string email);
        User CreateUser(string email, string name, string password);
    }
}
