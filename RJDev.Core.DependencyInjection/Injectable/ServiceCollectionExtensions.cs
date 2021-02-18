using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register implementations from given assemblies
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblies"></param>
        public static IServiceCollection WithInjectablesFrom(this IServiceCollection serviceCollection, params Assembly[] assemblies)
        {
            InjectableRegistrar.RegisterFromAssemblies(serviceCollection, assemblies);
            return serviceCollection;
        }
    }
}