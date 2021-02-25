using System;

namespace RJDev.Core.Command.Tests.Commands.FirstCommandSet
{
    [Command("bye")]
    public class ByeCommand : ICommand
    {
        /// <inheritdoc />
        public void Execute(params object[] args)
        {
            Console.Write("Bye");

            if (args.Length > 0)
            {
                Console.Write(args[0]);
            }

            Console.WriteLine();
        }

        /// <inheritdoc />
        public string GetDetails()
        {
            return $@"Usage: bye <optional text>

Arguments:
{"\t"}<optional text>{"\t"}Name of something you want to say bye to.";
        }
    }
}