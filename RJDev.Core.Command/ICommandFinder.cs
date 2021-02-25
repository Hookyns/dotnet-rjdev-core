using System;
using System.Collections.Generic;

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
        /// Return all Command instances
        /// </summary>
        /// <param name="belongsTo"></param>
        /// <returns></returns>
        IEnumerable<CommandDescriptor> GetCommands(Type? belongsTo = null);
    }
}