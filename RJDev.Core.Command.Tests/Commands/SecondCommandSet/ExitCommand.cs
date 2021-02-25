using System;

namespace RJDev.Core.Command.Tests.Commands.SecondCommandSet
{
    [Command("exit", belongsTo: typeof(Holders.SpecificCommandSet))]
    public class ExitCommand : ICommand
    {
        /// <inheritdoc />
        public void Execute(params object[] args)
        {
            Console.WriteLine("Exiting application...");
        }

        /// <inheritdoc />
        public string GetDetails()
        {
            return "Usage: exit";
        }
    }
}