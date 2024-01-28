using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RJDev.Core.DependencyInjection.Injectable;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Register implementations from given assemblies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="assemblies"></param>
    public static IServiceCollection AddInjectables(
        this IServiceCollection serviceCollection,
        params Assembly[] assemblies
    )
    {
        var injectableDescriptors = InjectablesFinder.GetInjectableTypes(assemblies);

        foreach (ServiceDescriptor dependency in ToServiceDescriptors(injectableDescriptors))
        {
            serviceCollection.Add(dependency);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Register implementations from given assemblies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="selector"></param>
    /// <param name="assemblies"></param>
    public static IServiceCollection AddInjectables(
        this IServiceCollection serviceCollection,
        Func<InjectableDescriptor, bool> selector,
        params Assembly[] assemblies
    )
    {
        var injectableDescriptors = InjectablesFinder.GetInjectableTypes(assemblies).Where(selector);

        foreach (ServiceDescriptor dependency in ToServiceDescriptors(injectableDescriptors))
        {
            serviceCollection.Add(dependency);
        }

        return serviceCollection;
    }

    /// <summary>
    /// Register implementations from given assemblies
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="selector"></param>
    /// <param name="assemblies"></param>
    public static IServiceCollection AddInjectables(
        this IServiceCollection serviceCollection,
        Func<IEnumerable<InjectableDescriptor>, IEnumerable<ServiceDescriptor>> selector,
        params Assembly[] assemblies
    )
    {
        foreach (var descriptor in selector(InjectablesFinder.GetInjectableTypes(assemblies)))
        {
            serviceCollection.Add(descriptor);
        }

        return serviceCollection;
    }

    private static IEnumerable<ServiceDescriptor> ToServiceDescriptors(IEnumerable<InjectableDescriptor> descriptors)
    {
        return descriptors.SelectMany(x =>
            x.Services.Select(service =>
                new ServiceDescriptor(
                    service.Service,
                    x.Implementation,
                    service.ServiceLifetime ?? ServiceLifetime.Transient
                )));
    }
}