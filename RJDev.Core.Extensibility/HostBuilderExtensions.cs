using System;
using System.Linq;
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
            IAddon[] addons = GetAddons(assemblyFinder);

            hostBuilder.ConfigureServices((_, serviceCollection) =>
            {
                // Register AddonRunner which will call the Configure methods of addons.
                serviceCollection.AddHostedService(provider => new AddonRunner(
                    provider.GetRequiredService<IHostEnvironment>(),
                    provider.GetRequiredService<IConfiguration>(),
                    provider,
                    addons
                ));

                // Configure addon services
                foreach (IAddon addon in addons)
                {
                    try
                    {
                        addon.ConfigureServices(serviceCollection);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"'ConfiguraceServices' method call of addon '{addon.GetType().FullName}' failed.", ex);
                    }
                }
            });

            return hostBuilder;
        }

        /// <summary>
        /// Returns instances of addons
        /// </summary>
        /// <param name="assemblyFinder"></param>
        private static IAddon[] GetAddons(IAssemblyFinder? assemblyFinder)
        {
            assemblyFinder ??= new DefaultAssemblyFinder();

            Type addonInterfaceType = typeof(IAddon);

            return assemblyFinder.GetAssemblies("*.dll")
                .SelectMany(assembly => assembly
                    .GetExportedTypes()
                    .Where(type => type.IsClass && !type.IsAbstract && addonInterfaceType.IsAssignableFrom(type))
                )
                .Select(addonType => (IAddon) (
                    Activator.CreateInstance(addonType) ?? throw new NullReferenceException($"Unable no create instance of addon '{addonType.FullName}'."))
                )
                .ToArray();
        }
    }
}