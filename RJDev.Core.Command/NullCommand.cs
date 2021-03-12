namespace RJDev.Core.Command
{
    /// <summary>
    /// Null command doing nothing. It is a fallback when no command found by CommandFinder.
    /// </summary>
    public class NullCommand : ICommand
    {
        /// <summary>
        /// Name of the command requested but does not exist
        /// </summary>
        public string RequestedCommandName { get; set; } = string.Empty;

        /// <inheritdoc />
        public void Execute(params object[] args)
        {
            // Is this good idea? It's additional dependency for just this call.
            // this.logger.LogError($"Requested command {this.RequestedCommandName} does not exists. Null command applied.");
            
            // do nothing...
        }

        /// <inheritdoc />
        public string GetDetails()
        {
            return string.Empty;
        }
    }
}