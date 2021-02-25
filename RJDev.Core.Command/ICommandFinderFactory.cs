using System.Reflection;

namespace RJDev.Core.Command
{
    /// <summary>
    /// Command finder factory interface
    /// </summary>
    public interface ICommandFinderFactory
    {
        /// <summary>
        /// Returns instance of command finder over given assemblies
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        ICommandFinder CreateCommandFinder(params Assembly[] assemblies);
    }
}