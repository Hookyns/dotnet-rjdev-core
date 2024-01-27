using System;

namespace RJDev.Core.Command
{
    /// <summary>
    /// Attribute for marking Commands
    /// </summary>
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Command name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Type owning the command; eg. host service type
        /// </summary>
        public Type? BelongsTo { get; }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="belongsTo">Optional Type parameter. Command can be related to some type, eg. related to hosts, services,..</param>
        public CommandAttribute(string name, Type? belongsTo = null)
        {
            Name = name;
            BelongsTo = belongsTo;
        }
    }
}