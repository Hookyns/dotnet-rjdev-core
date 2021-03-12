namespace RJDev.Core.Command
{
    /// <summary>
    /// Factory for null command
    /// </summary>
    public interface INullCommandFactory
    {
        /// <summary>
        /// Create command
        /// </summary>
        /// <returns></returns>
        ICommand Create();
    }
}