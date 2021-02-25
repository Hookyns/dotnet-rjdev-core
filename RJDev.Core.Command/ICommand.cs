namespace RJDev.Core.Command
{
    /// <summary>
    /// Interface for Commands
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Execute command
        /// </summary>
        void Execute(params object[] args);

        /// <summary>
        /// Return details about command as help text
        /// </summary>
        string GetDetails();
    }
}
