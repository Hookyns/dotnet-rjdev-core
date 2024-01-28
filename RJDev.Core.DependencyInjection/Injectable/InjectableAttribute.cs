using System;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable;

/// <summary>
/// Attribute marking default implementations of services which should not be registered manually.
/// Each registration can be overriden by different implementations.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class InjectableAttribute : Attribute
{
    /// <summary>
    /// Service (dependency) type
    /// </summary>
    public Type? Service { get; }

    /// <summary>
    /// Service lifetime
    /// </summary>
    public ServiceLifetime? ServiceLifetime { get; }

    /// <summary>
    /// Register this implementation as default for all interfaces it implements.
    /// Inherited interfaces are not included.
    /// </summary>
    public InjectableAttribute()
    {
    }

    /// <summary>
    /// Register this implementation as default for all interfaces it implements.
    /// Inherited interfaces are not included.
    /// </summary>
    /// <param name="serviceLifetime"></param>
    public InjectableAttribute(ServiceLifetime serviceLifetime)
    {
        ServiceLifetime = serviceLifetime;
    }

    /// <summary>
    /// Register this implementation as default for given service type.
    /// </summary>
    /// <param name="service"></param>
    public InjectableAttribute(Type service)
    {
        Service = service;
    }

    /// <summary>
    /// Register this implementation as default for given service type.
    /// </summary>
    /// <param name="service"></param>
    /// <param name="serviceLifetime"></param>
    public InjectableAttribute(Type service, ServiceLifetime serviceLifetime)
    {
        Service = service;
        ServiceLifetime = serviceLifetime;
    }
}