using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.Command
{
    /// <summary>
    /// Default Command finder factory implementation
    /// </summary>
    public class CommandFinderFactory : ICommandFinderFactory
    {
        /// <summary>
        /// Service provider instance
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        public CommandFinderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public ICommandFinder CreateCommandFinder(params Assembly[] assemblies)
        {
            return ActivatorUtilities.CreateInstance<CommandFinder>(_serviceProvider, (object)assemblies);
        }
    }
}