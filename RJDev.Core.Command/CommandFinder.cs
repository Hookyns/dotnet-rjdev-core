using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.Command
{
    /// <summary>
    /// Command finder
    /// </summary>
    public class CommandFinder : ICommandFinder
    {
        /// <summary>
        /// Collection of Commands
        /// </summary>
        private readonly Lazy<IEnumerable<CmdInfo>> cmdTypes;

        /// <summary>
        /// Instance of service provider
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="assemblies">Source assemblies</param>
        public CommandFinder(IServiceProvider serviceProvider, IEnumerable<Assembly> assemblies)
        {
            this.serviceProvider = serviceProvider;
            this.cmdTypes = new Lazy<IEnumerable<CmdInfo>>(() => GetCommandTypes(assemblies));
        }

        /// <inheritdoc />
        public ICommand GetCommand(string name, Type? belongsTo)
        {
            CmdInfo cmdInfo = this.cmdTypes.Value
                .First(x => x.Attr.Name == name && x.Attr.BelongsTo == belongsTo);

            return this.GetCommandInstance(cmdInfo);
        }

        /// <inheritdoc />
        public IEnumerable<CommandDescriptor> GetCommands(Type? belongsTo = null)
        {
            return this.cmdTypes.Value
                .Where(cmdInfo => cmdInfo.Attr.BelongsTo == belongsTo)
                .Select(cmdInfo => new CommandDescriptor(
                        cmdInfo.Attr.Name,
                        this.GetCommandInstance(cmdInfo)
                    )
                );
        }

        /// <summary>
        /// Return instance of Command described by given info object
        /// </summary>
        /// <param name="cmdInfo"></param>
        /// <returns></returns>
        private ICommand GetCommandInstance(CmdInfo cmdInfo)
        {
            Type commandFinderType = typeof(ICommandFinder);
            
            bool requiresCommandFinder = cmdInfo.Type.GetConstructors()
                .Any(constructorInfo => constructorInfo
                    .GetParameters()
                    .Any(param => commandFinderType.IsAssignableFrom(param.ParameterType)
                    )
                );

            ICommand? command;

            if (requiresCommandFinder)
            {
                command = ActivatorUtilities.CreateInstance(this.serviceProvider, cmdInfo.Type, this) as ICommand;
            }
            else
            {
                command = this.serviceProvider.GetService(cmdInfo.Type) as ICommand;
            }

            if (command == null)
            {
                throw new InvalidOperationException($"Registered service of type '{cmdInfo.Type.FullName}' is not a ICommand.");
            }

            return command;
        }

        /// <summary>
        /// Return Command types from given assemblies
        /// </summary>
        /// <returns></returns>
        private static IEnumerable<CmdInfo> GetCommandTypes(IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(a => a.GetTypes())
                .Where(x => x.IsClass && !x.IsAbstract)
                .Select(t => new
                {
                    Type = t,
                    Attr = t.GetCustomAttributes<CommandAttribute>(true).FirstOrDefault()
                })
                .Where(x => x.Attr != null)
                .Select(x => new CmdInfo(x.Type, x.Attr!));
        }

        /// <summary>
        /// Internal info about commands found
        /// </summary>
        private class CmdInfo
        {
            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="type"></param>
            /// <param name="attr"></param>
            public CmdInfo(Type type, CommandAttribute attr)
            {
                this.Type = type;
                this.Attr = attr;
            }

            /// <summary>
            /// Command type
            /// </summary>
            internal Type Type { get; }

            /// <summary>
            /// Attribute
            /// </summary>
            internal CommandAttribute Attr { get; }
        }
    }
}