using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
        public bool TryGetCommand(string name, [MaybeNullWhen(false)] out ICommand command);

        /// <summary>
        /// Return true if command found and set Command instance into output parameter
        /// </summary>
        /// <param name="name"></param>
        /// <param name="belongsTo"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool TryGetCommand(string name, Type? belongsTo, [MaybeNullWhen(false)] out ICommand command);
        
        /// <summary>
        /// Return all Command instances
        /// </summary>
        /// <param name="belongsTo"></param>
        /// <returns></returns>
        IEnumerable<CommandDescriptor> GetCommands(Type? belongsTo = null);
    }
}