using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RJDev.Core.Reflection.AssemblyFinder;

namespace RJDev.Core.Extensibility
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Run application with Addons
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="assemblyFinder"></param>
        public static IHostBuilder WithAddons(this IHostBuilder hostBuilder, IAssemblyFinder? assemblyFinder = null)
        {
            IEnumerable<Assembly> assemblies = GetAssemblies(assemblyFinder);
            IAddon[] addons = GetAddons(assemblies);
            return WithAddons(hostBuilder, addons);
        }

        /// <summary>
        /// Run application with Addons.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="assemblies"></param>
        public static IHostBuilder WithAddons(this IHostBuilder hostBuilder, Assembly[] assemblies)
        {
            IAddon[] addons = GetAddons(assemblies);
            return WithAddons(hostBuilder, addons);
        }

        /// <summary>
        /// Run application with Addons.
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="addons"></param>
        private static IHostBuilder WithAddons(IHostBuilder hostBuilder, IAddon[] addons)
        {
            // Configure addon services
            foreach (IAddon addon in addons)
            {
                try
                {
                    addon.Configure(hostBuilder);
                }
                catch (Exception ex)
                {
                    throw new Exception($"'Configure' method call of addon '{addon.GetType().FullName}' failed.", ex);
                }
            }

            hostBuilder.ConfigureServices((_, serviceCollection) =>
            {
                // Register AddonRunner which will call the Configure methods of addons.
                serviceCollection.AddHostedService(provider => new AddonRunner(
                    provider.GetRequiredService<IHostEnvironment>(),
                    provider.GetRequiredService<IConfiguration>(),
                    provider,
                    addons
                ));
            });

            return hostBuilder;
        }

        /// <summary>
        /// Returns assemblies where Addons will be searched.
        /// </summary>
        /// <param name="assemblyFinder"></param>
        private static IEnumerable<Assembly> GetAssemblies(IAssemblyFinder? assemblyFinder)
        {
            assemblyFinder ??= new DefaultAssemblyFinder();
            return assemblyFinder.GetAssemblies("*.dll");
        }

        /// <summary>
        /// Returns instances of Addons.
        /// </summary>
        /// <param name="assemblies"></param>
        private static IAddon[] GetAddons(IEnumerable<Assembly> assemblies)
        {
            Type addonInterfaceType = typeof(IAddon);

            return assemblies
                .SelectMany(assembly => assembly
                    .GetExportedTypes()
                    .Where(type => type.IsClass && !type.IsAbstract && addonInterfaceType.IsAssignableFrom(type))
                )
                .Select(addonType => (IAddon)(
                    Activator.CreateInstance(addonType) ?? throw new NullReferenceException($"Unable no create instance of addon '{addonType.FullName}'."))
                )
                .ToArray();
        }
    }
}