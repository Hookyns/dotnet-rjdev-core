using System;
using Microsoft.Extensions.DependencyInjection;

namespace RJDev.Core.DependencyInjection.Injectable;

/// <summary>
/// Describes a service to be registered in the DI container, including its type and optional lifetime.
/// </summary>
/// <param name="Service"></param>
/// <param name="ServiceLifetime"></param>
public record InjectableServiceDescriptor(Type Service, ServiceLifetime? ServiceLifetime);
