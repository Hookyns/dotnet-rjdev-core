# RJDev.Core.Essentials
Eseeentials you need in every project.

#### Registering with IServiceCollection
```csharp
serviceCollection.AddRJDevCoreEssentials();
```

## AppString

> Object representing string inside an application.

```csharp
record AppString(string Id, string? Description = null)
```

- **Id** - Text/message/error identifier which should be unique. Can be used as localization key.
- **Description** - Default text of the message.

The `Id` property is important. This whole object can be sent from API to the FE, external services etc.
It is possible to identify each AppString and create custom localizations for each unique `Id`.
`Description` contains default message eg. in english.


### Example
```csharp
public static class AuthAppStrings
{
    public static class Register
    {
        public static readonly AppString CreationFailed = new("Auth.Register.CreationFailed", "Account creation failed.");
    }

    public static class Auth
    {
        public static readonly AppString InvalidCredentials = new("Auth.Auth.InvalidCredentials", "Invalid credentials.");
        public static readonly AppString NotAuthorized = new("Auth.Auth.NotAuthorized", "Your account is not authorized.");
        public static readonly AppString Blocked = new("Auth.Auth.Blocked", "Your account has been blocked.");
    }
}

class Something {
    public AppString Auth(Some thing) {
        if (/* ... */) return AuthAppStrings.Auth.InvalidCredentials;
        // ...
        if (!user.Authorized) return AuthAppStrings.Auth.NotAuthorized;
        // ...
    }
}
```


### IAppStringFinder
If you use AppString like the example above, you can use `IAppStringFinder` to find all the AppStrings in your application, so you can handle them in some way (eg. send them to FE, create localization files etc.).

```csharp
/// <summary>
/// Interface for the service looking for all the statically declared <see cref="AppString"/> in application
/// so we know all the messages used in source code.
/// </summary>
public interface IAppStringFinder
{
	/// <summary>
	/// Returns collection of all the <see cref="AppString"/> declared in the application.
	/// </summary>
	/// <param name="cached">Use cached rasult or look for all the AppStrings again.</param>
	/// <param name="assemblies">Which assemblies should be used. All assemblies in the bin will be used if no assemblies specified.</param>
	/// <returns></returns>
	IEnumerable<AppString> GetAllAppStrings(bool cached = true, Assembly[]? assemblies = null);

	/// <summary>
	/// Return <see cref="AppString"/> by its <see cref="AppString.Id"/> property.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="assemblies">Which assemblies should be used. All assemblies in the bin will be used if no assemblies specified.</param>
	/// <returns></returns>
	AppString GetAppString(string id, Assembly[]? assemblies = null);
}
```

## Results
Types representing result of some operation. Interfaces `IResult`, `IResult<T>` and implementations `Result`, `Result<T>`.

### Example
```csharp
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
```