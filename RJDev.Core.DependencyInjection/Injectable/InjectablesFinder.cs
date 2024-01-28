using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable;

public static class InjectablesFinder
{
    public static IEnumerable<InjectableDescriptor> GetInjectableTypes(IEnumerable<Assembly> assemblies)
    {
        return assemblies.SelectMany(a => a.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract)
            .Select(type =>
            {
                var services = new Dictionary<Type, ServiceLifetime?>();

                foreach (InjectableAttribute attribute in type.GetCustomAttributes<InjectableAttribute>(inherit: true))
                {
                    if (attribute.Service != null)
                    {
                        services.Add(attribute.Service, attribute.ServiceLifetime);
                    }
                    else
                    {
                        foreach (Type implementedInterface in type.GetTypeInfo().ImplementedInterfaces)
                        {
                            services.Add(implementedInterface, attribute.ServiceLifetime);
                        }
                    }
                }

                if (services.Count == 0)
                {
                    return null;
                }

                return new InjectableDescriptor(type,
                    services.Select(entry => new InjectableServiceDescriptor(entry.Key, entry.Value)));
            })
            .Where(x => x != null)!;
    }
}