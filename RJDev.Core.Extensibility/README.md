# RJDev.Core.Extensibility
Extensibility module of RJDev.Core library.

## Example
```c#
public class TestAddon : IAddon
{
    public void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ISomeService, SomeService>();
    }

    public void Configure(IHostEnvironment hostingEnvironment, IConfiguration configuration, IServiceProvider serviceProvider)
    {
        serviceProvider.GetRequiredService<ISomeService>().ConfigureCalled = true;
    }
}
```

```c#
Host.CreateDefaultBuilder()
    .WithAddons()
    .ConfigureServices((_, _) => { })
```

For more examples see Tests.