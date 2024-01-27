namespace RJDev.Core.Command
{
    /// <summary>
    /// Command descriptor
    /// </summary>
    public class CommandDescriptor
    {
        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Instance of command
        /// </summary>
        public ICommand Command { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="command"></param>
        public CommandDescriptor(string name, ICommand command)
        {
            Name = name;
            Command = command;
        }
    }
}