using System;
using System.Collections.Generic;
using System.Linq;

namespace RJDev.Core.Command.Tests.Commands.SecondCommandSet
{
    [Command("list", belongsTo: typeof(Holders.SpecificCommandSet))]
    public class ListCommand : ICommand
    {
        /// <summary>
        /// Command finder
        /// </summary>
        private readonly ICommandFinder _commandFinder;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="commandFinder"></param>
        public ListCommand(ICommandFinder commandFinder)
        {
            _commandFinder = commandFinder;
        }

        /// <inheritdoc />
        public void Execute(params object[] args)
        {
            // Get OUT list
            List<string> outList = args[0] as List<string> 
                ?? throw new InvalidOperationException("This command requires one arguments of type 'List<string>'");
            
            // List commands from scoped command finder
            IEnumerable<string> commands = _commandFinder.GetCommands().Select(c => c.Command.GetDetails());

            // Put commands into OUT list
            outList.AddRange(commands);
        }

        /// <inheritdoc />
        public string GetDetails()
        {
            return "Usage: list";
        }
    }
}