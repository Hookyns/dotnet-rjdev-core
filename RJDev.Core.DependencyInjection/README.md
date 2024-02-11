# RJDev.Core.Reflection
DependencyInjection module of RJDev.Core library.

## InjectableAttribute
The `Injectable` attribute is used for registration of default implementations. 
No need to write thousands of lines of service registrations across projects. 
Sure you can do some kind of matching by convention (like register all "*Service" classes), but it's not always possible. 
I personally do not name everything XxxService. I prefer specific names like XxxProvider, XxxManager, XxxClient, XxxReader, XxxBuilder, XxxMapper, XxxValidator, XxxDecorator, XxxAdapter, XxxPublisher, XxxConsumer, XxxProcessor, XxxDispatcher, XxxChannel, XxxRouter, XxxGenerator, XxxProducer, etc. 
So in this case, I would have to write a lot of registrations.

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