using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RJDev.Core.Command.Tests.Commands.FirstCommandSet;
using RJDev.Core.Command.Tests.Commands.SecondCommandSet;
using RJDev.Core.Command.Tests.Holders;
using Xunit;
using Xunit.Abstractions;

namespace RJDev.Core.Command.Tests
{
    public class CommandTest
    {
        /// <summary>
        /// Output helper
        /// </summary>
        private readonly ITestOutputHelper testOutputHelper;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="testOutputHelper"></param>
        public CommandTest(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        /// <summary>
        /// Test of finding commands not belonging to any type
        /// </summary>
        [Fact]
        public void DefaultCommandsFound()
        {
            IServiceProvider provider = GetServiceProvider();
            ICommandFinder finder = GetCommandFinder(provider);

            IEnumerable<CommandDescriptor> commands = finder.GetCommands().OrderBy(c => c.Name);

            Assert.Collection(
                commands,
                descriptor => Assert.IsType<ByeCommand>(descriptor.Command),
                descriptor => Assert.IsType<SayCommand>(descriptor.Command)
            );
        }

        /// <summary>
        /// Test of finding commands belonging to type
        /// </summary>
        [Fact]
        public void BelongingCommandsFound()
        {
            IServiceProvider provider = GetServiceProvider();
            ICommandFinder finder = GetCommandFinder(provider);

            IEnumerable<CommandDescriptor> commands = finder.GetCommands(typeof(SpecificCommandSet)).OrderBy(c => c.Name);

            Assert.Collection(
                commands,
                descriptor => Assert.IsType<ExitCommand>(descriptor.Command),
                descriptor => Assert.IsType<ListCommand>(descriptor.Command)
            );
        }

        /// <summary>
        /// Test of ListCommand - testing dependency injection of ICommandFinder
        /// </summary>
        [Fact]
        public void ListCommand()
        {
            IServiceProvider provider = GetServiceProvider();
            ICommandFinder finder = GetCommandFinder(provider);
            
            List<string> commands = new ();
            ICommand command = finder.GetCommand("list", belongsTo: typeof(SpecificCommandSet));
            command.Execute(commands);
            
            this.testOutputHelper.WriteLine(string.Join(Environment.NewLine, commands));
            
            Assert.Collection(
                commands.OrderBy(x => x),
                cmdDetails => Assert.Equal("Usage: bye", cmdDetails[..10]),
                cmdDetails => Assert.Equal("Usage: say", cmdDetails[..10])
                );
        }

        /// <summary>
        /// Return command finder
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        private static ICommandFinder GetCommandFinder(IServiceProvider provider)
        {
            ICommandFinderFactory factory = provider.GetRequiredService<ICommandFinderFactory>();
            ICommandFinder finder = factory.CreateCommandFinder(new[] {typeof(CommandTest).Assembly});
            return finder;
        }

        /// <summary>
        /// Prepare IServiceProvider
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider GetServiceProvider()
        {
            ServiceCollection collection = new();
            collection.AddSingleton<ICommandFinderFactory, CommandFinderFactory>();
            collection.AddScoped<ICommandFinder, CommandFinder>();

            collection.AddTransient<SayCommand>();
            collection.AddTransient<ByeCommand>();
            collection.AddTransient<ListCommand>();
            collection.AddTransient<ExitCommand>();

            ServiceProvider provider = collection.BuildServiceProvider();
            return provider;
        }
    }
}