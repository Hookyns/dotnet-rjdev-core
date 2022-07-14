using System;
using System.Collections.Generic;
#if !NETSTANDARD2_0
using System.Diagnostics.CodeAnalysis;
#endif

namespace RJDev.Core.Command
{
    /// <summary>
    /// Command finder interface
    /// </summary>
    public interface ICommandFinder
    {
        /// <summary>
        /// Return Command instance
        /// </summary>
        /// <param name="name"></param>
        /// <param name="belongsTo"></param>
        /// <returns></returns>
        ICommand GetCommand(string name, Type? belongsTo = null);

        /// <summary>
        /// Return true if command found and set Command instance into output parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="command"></param>
        /// <returns></returns>
#if NETSTANDARD2_0
        public bool TryGetCommand(string name, out ICommand command);
#else
        public bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command);
#endif

        /// <summary>
        /// Return true if command found and set Command instance into output parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="belongsTo"></param>
        /// <param name="command"></param>
        /// <returns></returns>
#if NETSTANDARD2_0
        public bool TryGetCommand(string name, Type? belongsTo, out ICommand command);
#else
        public bool TryGetCommand(string name, Type? belongsTo, [MaybeNullWhen(false)] out ICommand command);
#endif

        /// <summary>
        /// Return all Command instances
        /// </summary>
        /// <param name="belongsTo"></param>
        /// <returns></returns>
        IEnumerable<CommandDescriptor> GetCommands(Type? belongsTo = null);
    }
}