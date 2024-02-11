using RJDev.Core.Essentials.AppStrings;
using RJDev.Core.Essentials.Results;
using Xunit;

namespace RJDev.Core.Essentials.Tests;

public static class AuthErrors
{
    public static readonly AppString InvalidCredentials = new("Auth.InvalidCredentials", "Invalid credentials.");

    public static readonly AppString AccountNotAuthorized =
        new("Auth.AccountNotAuthorized", "Your account is not authorized.");
}

public class DemoTests
{
    record User(string Name, bool Authorized);

    class AuthService
    {
        public IResult<User> Login(string username, string password)
        {
            if (username != "test" && password != "test")
            {
                return Result.Error<User>(AuthErrors.InvalidCredentials);
            }

            var user = new User(username, true);

            if (!user.Authorized)
            {
                return Result.Error<User>(AuthErrors.AccountNotAuthorized);
            }

            return Result.Ok(user);
        }
    }

    [Fact]
    public void AuthTest()
    {
        var authService = new AuthService();
        var user = authService.Login("test", "test");

        if (!user.IsOk)
        {
            Assert.Fail();
        }

        Assert.Equal("test", user.Value.Name);
        Assert.True(user.Value.Authorized);
    }
}