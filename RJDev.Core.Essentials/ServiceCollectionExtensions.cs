using RJDev.Core.Essentials.AppStrings;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRJDevCoreEssentials(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IAppStringFinder, AppStringFinder>();
    }
}