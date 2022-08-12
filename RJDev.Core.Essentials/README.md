# RJDev.Core.Essentials
Application utils.

## AppString
```csharp
/// <summary>
/// Object representing string inside an application.
/// </summary>
/// <remarks>
/// The <see cref="Id"/> property is important. This whole object can be sent from API to the FE, external services etc.
/// It is possible to identify each AppString and create custom localizations for each unique <see cref="Id"/>.
/// <see cref="Description"/> contains default message eg. in english.
/// </remarks>
/// <param name="Id">Text/message/error identifier which should be unique. Can be used as localization key.</param>
/// <param name="Description">Default text of the message.</param>
/// <exception cref="ArgumentNullException">If <see cref="Id"/> is null.</exception>
public record AppString(string Id, string? Description = null)
{
    /// <summary/>
    public string Id { get; } = Id ?? throw new ArgumentNullException(nameof(Id));
    
    /// <summary/>
    public string Description { get; } = Description ?? string.Empty;
}
```

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