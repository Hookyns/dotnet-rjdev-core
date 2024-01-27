using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        private readonly Lazy<IEnumerable<CmdInfo>> _cmdTypes;

        /// <summary>
        /// Instance of service provider
        /// </summary>
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="assemblies">Source assemblies</param>
        public CommandFinder(IServiceProvider serviceProvider, IEnumerable<Assembly> assemblies)
        {
            _serviceProvider = serviceProvider;
            _cmdTypes = new Lazy<IEnumerable<CmdInfo>>(() => GetCommandTypes(assemblies));
        }

        /// <inheritdoc />
        public ICommand GetCommand(string name, Type? belongsTo)
        {
            CmdInfo? cmdInfo = _cmdTypes.Value
                .FirstOrDefault(x => x.Attr.Name == name && x.Attr.BelongsTo == belongsTo);

            if (cmdInfo == null)
            {
                return GetNullCommandInstance(name);
            }

            return GetCommandInstance(cmdInfo);
        }

        /// <inheritdoc />
        public bool TryGetCommand(
            string name, 
#if !NETSTANDARD2_0
            [MaybeNullWhen(false)] 
#endif
            out ICommand command)
        {
            return TryGetCommand(name, null, out command);
        }

        /// <inheritdoc />
        public bool TryGetCommand(
            string name, 
            Type? belongsTo, 
#if !NETSTANDARD2_0
            [MaybeNullWhen(false)] 
#endif
            out ICommand command)
        {
            CmdInfo? cmdInfo = _cmdTypes.Value.FirstOrDefault(x => x.Attr.Name == name && x.Attr.BelongsTo == belongsTo);

            if (cmdInfo == null)
            {
                command = null;
                return false;
            }

            command = GetCommandInstance(cmdInfo);
            return true;
        }

        /// <inheritdoc />
        public IEnumerable<CommandDescriptor> GetCommands(Type? belongsTo = null)
        {
            return _cmdTypes.Value
                .Where(cmdInfo => cmdInfo.Attr.BelongsTo == belongsTo)
                .Select(cmdInfo => new CommandDescriptor(
                        cmdInfo.Attr.Name,
                        GetCommandInstance(cmdInfo)
                    )
                );
        }

        /// <summary>
        /// Returns instance of null command
        /// </summary>
        /// <param name="requestedCommandName"></param>
        /// <returns></returns>
        private ICommand GetNullCommandInstance(string requestedCommandName)
        {
            INullCommandFactory? nullCommandFactory = _serviceProvider.GetService<INullCommandFactory>();

            if (nullCommandFactory != null)
            {
                return nullCommandFactory.Create();
            }

            return new NullCommand()
            {
                RequestedCommandName = requestedCommandName
            };
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
                command = ActivatorUtilities.CreateInstance(_serviceProvider, cmdInfo.Type, this) as ICommand;
            }
            else
            {
                command = _serviceProvider.GetService(cmdInfo.Type) as ICommand;
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
                Type = type;
                Attr = attr;
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