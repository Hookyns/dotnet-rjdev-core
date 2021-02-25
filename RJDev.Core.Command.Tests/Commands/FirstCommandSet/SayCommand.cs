using System;

namespace RJDev.Core.Command.Tests.Commands.FirstCommandSet
{
    [Command("say")]
    public class SayCommand : ICommand
    {
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
}