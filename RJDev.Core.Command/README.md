# RJDev.Core.Command
Command module of RJDev.Core library.

Command is simple pattern of executable classes with injected dependencies and no output.

```c#
public interface ICommandFinderFactory
{
    /// <summary>
    /// Returns instance of command finder over given assemblies
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    ICommandFinder CreateCommandFinder(params Assembly[] assemblies);
}
```

```c#
public interface ICommandFinder
{
    /// <summary>
    /// Return Command instance
    /// </summary>
    /// <param name="name"></param>
    /// <param name="belongsTo"></param>
    /// <returns></returns>
    ICommand GetCommand(string name, Type? belongsTo = null);

    /// <summary>
    /// Return true if command found and set Command instance into output parameter
    /// </summary>
    /// <param name="name"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command);

    /// <summary>
    /// Return true if command found and set Command instance into output parameter
    /// </summary>
    /// <param name="name"></param>
    /// <param name="belongsTo"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    public bool TryGetCommand(string name, Type? belongsTo, [MaybeNullWhen(false)] out ICommand command);

    /// <summary>
    /// Return all Command instances
    /// </summary>
    /// <param name="belongsTo"></param>
    /// <returns></returns>
    IEnumerable<CommandDescriptor> GetCommands(Type? belongsTo = null);
}
```

## Example
```c#
[Command("say")]
public class SayCommand : ICommand
{
    public SayCommand(ISomeService someService) 
    {
        // ...
    }

    /// <inheritdoc />
    public void Execute(params object[] args)
    {
        Console.WriteLine(string.Join(' ', args));
    }

    /// <inheritdoc />
    public string GetDetails()
    {
        return $@"Usage: say <text>

Arguments:
{"\t"}<text>{"\t"}Some text.";
    }
}
```

```c#
public class Something()
{
    Something(ICommandFinderFactory commandFinderFactory)
    {
        ICommandFinder commandFinder = commandFinderFactory.CreateCommandFinder(typeof(Something).Assembly);
        commandFinder.GetCommand("say").Execute();
    }
}
```

For more examples see Tests.