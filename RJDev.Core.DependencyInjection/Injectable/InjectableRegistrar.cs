using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable
{
    /// <summary>
    /// Registrator of Injectable dependencies
    /// </summary>
    public static class InjectableRegistrar
    {
        /// <summary>
        /// Register implementations from given assemblies into given service collection
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblies"></param>
        public static void RegisterFromAssemblies(IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            var dependencies = GetDependencies(assemblies);

            foreach (var dependency in dependencies)
            {
                serviceCollection.Add(dependency);
            }
        }

        /// <summary>
        /// Return list of dependencies
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        private static IEnumerable<ServiceDescriptor> GetDependencies(IEnumerable<Assembly> assemblies)
        {
            IEnumerable<InjectableImplementationInfo>? injectableImplementations = assemblies.SelectMany(a => a.GetTypes())
                .Where(type => type.IsClass && !type.IsAbstract)
                .Select(type => new
                {
                    type,
                    attribute = type.GetCustomAttribute<InjectableAttribute>(inherit: true)
                })
                .Where(info => info.attribute != null)
                .Select(x => new InjectableImplementationInfo(x.type, x.attribute!));

            return injectableImplementations
                .Select(info => new ServiceDescriptor(info.InjectableAttribute.Service, info.Type, info.InjectableAttribute.ServiceLifetime));
        }

        private class InjectableImplementationInfo
        {
            /// <summary>
            /// Ctor
            /// </summary>
            /// <param name="type"></param>
            /// <param name="injectableAttribute"></param>
            public InjectableImplementationInfo(Type type, InjectableAttribute injectableAttribute)
            {
                this.Type = type;
                this.InjectableAttribute = injectableAttribute;
            }

            /// <summary>
            /// Type of implementation
            /// </summary>
            public Type Type { get; }

            /// <summary>
            /// Instance of injectable attribute
            /// </summary>
            public InjectableAttribute InjectableAttribute { get; }
        }
    }
}