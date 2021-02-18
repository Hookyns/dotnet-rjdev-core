# RJDev.Core.Reflection
DependencyInjection module of RJDev.Core library.

## InjectableAttribute
The `Injectable` attribute is used for registration of default implementations. No need to write thousands lines of default implementation registrations somewhere across project. Speeds up creating a lot of classes.

```c#
interface ISomeService
{
    ...
}

[Injectable(typeof(ISomeService), ServiceLifetime.Transient)]
class SomeService : ISomeService
{
    ...
}
```

Registration of Injectables:
```c#
Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, serviceCollection) =>
    {
        // Register Injectable implementations
        InjectableRegistrar.RegisterFromAssemblies(serviceCollection, typeof(Program).Assembly);
        
        // or
        serviceCollection.WithInjectablesFrom(typeof(Program).Assembly);
    })
```