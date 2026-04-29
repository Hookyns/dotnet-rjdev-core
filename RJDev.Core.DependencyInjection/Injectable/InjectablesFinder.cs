using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable;

/// <summary>
/// Scans assemblies for classes decorated with [Injectable] and extracts their service types and lifetimes.
/// </summary>
public static class InjectablesFinder
{
    /// <summary>
    /// Scans the provided assemblies for classes decorated with [Injectable] attributes and
    /// returns a collection of InjectableDescriptor objects that describe the injectable types,
    /// their service types, and lifetimes.
    /// </summary>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IEnumerable<InjectableDescriptor> GetInjectableTypes(
        IEnumerable<Assembly> assemblies
    )
    {
        return assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .Select(type =>
            {
                var services = new Dictionary<Type, ServiceLifetime?>();

                foreach (
                    InjectableAttribute attribute in type.GetCustomAttributes<InjectableAttribute>(
                        inherit: true
                    )
                )
                {
                    if (attribute.Service != null)
                    {
                        services.Add(attribute.Service, attribute.ServiceLifetime);
                    }
                    else
                    {
                        var disposable = typeof(IDisposable);
                        var interfaces = type.GetTypeInfo()
                            .ImplementedInterfaces.Where(i => i != disposable)
                            .ToList();

                        // If there is no interface, register as Self
                        if (interfaces.Count == 0)
                        {
                            services.Add(type, attribute.ServiceLifetime);
                        }

                        foreach (Type implementedInterface in interfaces)
                        {
                            services.Add(implementedInterface, attribute.ServiceLifetime);
                        }
                    }
                }

                if (services.Count == 0)
                {
                    return null;
                }

                return new InjectableDescriptor(
                    type,
                    services.Select(entry => new InjectableServiceDescriptor(
                        entry.Key,
                        entry.Value
                    ))
                );
            })
            .Where(x => x != null)!;
    }
}
